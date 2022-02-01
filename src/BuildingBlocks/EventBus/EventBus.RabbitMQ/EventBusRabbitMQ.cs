using System;
using System.Net.Sockets;
using System.Text;
using EventBus.Base;
using EventBus.Base.Events;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace EventBus.RabbitMQ
{
    public class EventBusRabbitMQ : BaseEventBus
    {
        private readonly RabbitMQPersistentConnection _persistentConnection;
        private readonly IModel _consumerChannel;

        public EventBusRabbitMQ(EventBusConfig eventBusConfig, IServiceProvider serviceProvider) : base(eventBusConfig,
            serviceProvider)
        {
            IConnectionFactory connectionFactory;
            if (eventBusConfig.Connection != null)
            {
                var connectionJson = JsonConvert.SerializeObject(eventBusConfig.Connection, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                connectionFactory = JsonConvert.DeserializeObject<ConnectionFactory>(connectionJson);
            }
            else
            {
                connectionFactory = new ConnectionFactory();
            }

            _persistentConnection =
                new RabbitMQPersistentConnection(connectionFactory, eventBusConfig.ConnectionRetryCount);
            _consumerChannel = CreateConsumerChannel();
            SubsManager.OnEventRemoved += SubsManager_OnEventRemoved;
        }

        private void SubsManager_OnEventRemoved(object? sender, string eventName)
        {
            eventName = ProcessEventName(eventName);
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            _consumerChannel.QueueUnbind(eventName, EventBusConfig.DefaultTopicName, eventName);
            if (SubsManager.IsEmpty)
            {
                _consumerChannel.Close();
            }
        }

        public override void Publish(IntegrationEvent @event)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var policy = Policy.Handle<BrokerUnreachableException>().Or<SocketException>()
                .WaitAndRetry(5,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (ex, time) => { });
            var eventName = @event.GetType().Name;
            eventName = ProcessEventName(eventName);
            _consumerChannel.ExchangeDeclare(EventBusConfig.DefaultTopicName, "direct");

            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);

            policy.Execute(() =>
            {
                var properties = _consumerChannel.CreateBasicProperties();
                properties.DeliveryMode = 2; // persistent
                _consumerChannel.QueueDeclare(GetSubName(eventName), true, false, false, null);

                _consumerChannel.BasicPublish(EventBusConfig.DefaultTopicName, eventName, true, properties, body);
            });
        }

        public override void Subscribe<T, TH>()
        {
            var eventName = typeof(T).Name;
            eventName = ProcessEventName(eventName);
            if (!SubsManager.HasSubscriptionsForEvent(eventName))
            {
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }

                _consumerChannel.QueueDeclare(queue: GetSubName(eventName),
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                _consumerChannel.QueueBind(queue: GetSubName(eventName),
                    exchange: EventBusConfig.DefaultTopicName,
                    routingKey: eventName);
            }

            SubsManager.AddSubscription<T, TH>();
            StartBasicConsume(eventName);
        }

        public override void UnSubscribe<T, TH>()
        {
            SubsManager.RemoveSubscription<T, TH>();
        }

        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();
            channel.ExchangeDeclare(EventBusConfig.DefaultTopicName,
                "direct");
            return channel;
        }

        private void StartBasicConsume(string eventName)
        {
            if (_consumerChannel != null)
            {
                var consumer = new EventingBasicConsumer(_consumerChannel);
                consumer.Received += Consumer_Received;
                _consumerChannel.BasicConsume(GetSubName(eventName), false, consumer);
            }
        }

        private async void Consumer_Received(object? sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            eventName = ProcessEventName(eventName);
            var message = Encoding.UTF8.GetString(e.Body.Span);
            try
            {
                await ProcessEvent(eventName, message);
            }
            catch (Exception exception)
            {
                // logging
                Console.WriteLine(exception);
            }

            _consumerChannel.BasicAck(e.DeliveryTag, multiple: false);
        }
    }
}