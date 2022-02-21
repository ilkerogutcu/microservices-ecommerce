using System;
using EventBus.Base.Events;

namespace Order.API.IntegrationEvents.Events
{
    public class OrderStartedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; }
        public int OrderId { get; }

        public OrderStartedIntegrationEvent(Guid userId, int orderId)
        {
            UserId = userId;
            OrderId = orderId;
        }
    }
}