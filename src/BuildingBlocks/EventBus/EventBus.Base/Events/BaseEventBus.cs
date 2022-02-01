using System;
using System.Linq;
using System.Threading.Tasks;
using EventBus.Base.Abstraction;
using EventBus.Base.SubManagers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace EventBus.Base.Events
{
    public abstract class BaseEventBus : IEventBus
    {
        public readonly IServiceProvider ServiceProvider;
        public readonly IEventBusSubscriptionManager SubsManager;
        private EventBusConfig _eventBusConfig;

        protected BaseEventBus(EventBusConfig eventBusConfig, IServiceProvider serviceProvider)
        {
            _eventBusConfig = eventBusConfig;
            ServiceProvider = serviceProvider;
            SubsManager = new InMemoryEventBusSubscriptionManager(ProcessEventName);
        }

        public virtual string ProcessEventName(string eventName)
        {
            if (_eventBusConfig.DeleteEventPrefix)
            {
                eventName = eventName.TrimStart(_eventBusConfig.EventNamePrefix.ToArray());
            }

            if (_eventBusConfig.DeleteEventSuffix)
            {
                eventName = eventName.TrimEnd(_eventBusConfig.EventNameSuffix.ToArray());
            }

            return eventName;
        }

        public async Task<bool> ProcessEvent(string eventName, string message)
        {
            eventName = ProcessEventName(eventName);

            if (!SubsManager.HasSubscriptionsForEvent(eventName)) return false;
            var subscriptions = SubsManager.GetHandlersForEvent(eventName);
            using var scope = ServiceProvider.CreateScope();
            foreach (var subscription in subscriptions)
            {
                var handler = ServiceProvider.GetService(subscription.HandlerType);
                if (handler == null)
                {
                    continue;
                }

                var eventType =
                    SubsManager.GetEventTypeByName(
                        $"{_eventBusConfig.EventNamePrefix}{eventName}{_eventBusConfig.EventNameSuffix}");
                var integrationEvent = JsonConvert.DeserializeObject(message, eventType);

                var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                await (Task) concreteType.GetMethod("Handle")?.Invoke(handler, new[] {integrationEvent});
            }


            return true;
        }

        public virtual string GetSubName(string eventName)
        {
            return $"{_eventBusConfig.SubscriberClientAppName}.{ProcessEventName(eventName)}";
        }


        public virtual void Dispose()
        {
            _eventBusConfig = null;
        }

        public abstract void Publish(IntegrationEvent @event);
        public abstract void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;
        public abstract void UnSubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;
    }
}