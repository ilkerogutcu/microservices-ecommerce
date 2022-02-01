using System;
using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using EventBus.UnitTests.Events.EventHandlers;
using EventBus.UnitTests.Events.Events;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Xunit;

namespace EventBus.UnitTests
{
    public class EventBusTests
    {
        private ServiceCollection _services;

        public EventBusTests()
        {
            _services = new ServiceCollection();
        }
        [Fact]
        public void Subscribe_Event_On_RabbitMq_Test()
        {
            _services.AddSingleton<IEventBus>(sp =>
            {
                EventBusConfig config = new()
                {
                    ConnectionRetryCount = 5,
                    SubscriberClientAppName = "EventBus.UnitTests",
                    EventBusType = EventBusType.RabbitMQ,
                    EventNameSuffix = "IntegrationEvent",
                    // Connection = new ConnectionFactory()
                    // {
                    //     HostName = "localhost",
                    //     Port = 15672,
                    //     UserName = "guest",
                    //     Password = "guest"
                    // }
                };
                return EventBusFactory.Create(config,sp);
            });
            var sp = _services.BuildServiceProvider();
            var eventBus = sp.GetRequiredService<IEventBus>();
            eventBus.Subscribe<OrderCreatedIntegrationEvent,OrderCreatedIntegrationEventHandler>();
            eventBus.Publish(new OrderCreatedIntegrationEvent(5));
        }
    }
}