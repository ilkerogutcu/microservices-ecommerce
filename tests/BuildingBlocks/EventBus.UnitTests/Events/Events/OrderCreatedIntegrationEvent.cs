using EventBus.Base.Events;

namespace EventBus.UnitTests.Events.Events
{
    public class OrderCreatedIntegrationEvent:IntegrationEvent
    {
        public int Id { get; set; }

        public OrderCreatedIntegrationEvent(int id)
        {
            Id = id;
        }
    }
}