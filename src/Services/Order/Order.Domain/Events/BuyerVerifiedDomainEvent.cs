using System;
using MediatR;
using Order.Domain.AggregateModels.BuyerAggregate;

namespace Order.Domain.Events
{
    public class BuyerVerifiedDomainEvent : INotification
    {
        public Buyer Buyer { get; set; }
        public Guid OrderId { get; set; }

        public BuyerVerifiedDomainEvent(Buyer buyer, Guid orderId)
        {
            Buyer = buyer;
            OrderId = orderId;
        }
    }
}