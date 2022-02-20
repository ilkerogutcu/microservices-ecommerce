using System;
using MediatR;

namespace Order.Domain.Events
{
    public class OrderStartedDomainEvent : INotification
    {
        public string Email { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CardTypeId { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string CardSecurityNumber { get; set; }
        public DateTime CardExpiration { get; set; }
        public AggregateModels.OrderAggregate.Order Order { get; set; }

        public OrderStartedDomainEvent(string email, int cardTypeId, string cardNumber, string cardHolderName,
            string cardSecurityNumber, DateTime cardExpiration, AggregateModels.OrderAggregate.Order order, Guid userId, string firstName,
            string lastName)
        {
            Email = email;
            CardTypeId = cardTypeId;
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            CardSecurityNumber = cardSecurityNumber;
            CardExpiration = cardExpiration;
            Order = order;
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}