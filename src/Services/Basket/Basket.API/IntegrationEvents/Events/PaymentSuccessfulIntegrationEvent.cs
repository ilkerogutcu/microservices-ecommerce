using System;
using EventBus.Base.Events;

namespace Basket.API.IntegrationEvents.Events
{
    public class PaymentSuccessfulIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; set; }
    }
}