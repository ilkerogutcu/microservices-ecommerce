using System;
using EventBus.Base.Events;

namespace Order.Application.IntegrationEvents
{
    public class PaymentSuccessfulIntegrationEvent : IntegrationEvent
    {
        public PaymentSuccessfulIntegrationEvent(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; set; }
        
    }
}