using System.Threading.Tasks;
using EventBus.Base.Abstraction;
using EventBus.UnitTests.Events.Events;

namespace EventBus.UnitTests.Events.EventHandlers
{
    public class OrderCreatedIntegrationEventHandler:IIntegrationEventHandler<OrderCreatedIntegrationEvent>
    {
        public Task Handle(OrderCreatedIntegrationEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}