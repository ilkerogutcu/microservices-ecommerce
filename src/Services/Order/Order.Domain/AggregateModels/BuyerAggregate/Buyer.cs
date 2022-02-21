using System;
using System.Collections.Generic;
using System.Linq;
using Order.Domain.Events;
using Order.Domain.Exceptions;
using Order.Domain.SeedWork;

namespace Order.Domain.AggregateModels.BuyerAggregate
{
    public class Buyer : BaseEntity, IAggregateRoot
    {
        public string Email { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


        protected Buyer()
        {
            CreatedDate = DateTime.Now;
        }

        public Buyer(string email, Guid? userId, string firstName, string lastName) : this()
        {
            Email = email ?? throw new OrderingDomainException(nameof(email));
            UserId = userId ?? throw new OrderingDomainException(nameof(userId));
            FirstName = firstName ?? throw new OrderingDomainException(nameof(firstName));
            LastName = lastName ?? throw new OrderingDomainException(nameof(lastName));
        }

        public void VerifyBuyerMethod(Guid orderId)
        {
            AddDomainEvent(new BuyerVerifiedDomainEvent(this, orderId));
        }

        public override bool Equals(object obj)
        {
            return obj is Buyer buyer &&
                   Email == buyer.Email &&
                   UserId == buyer.UserId &&
                   FirstName == buyer.FirstName &&
                   LastName == buyer.LastName;
        }
    }
}