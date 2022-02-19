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
        public string FirstName { get; set; }
        public string LastName { get; set; }

        private List<PaymentMethod> _paymentMethods;
        public IReadOnlyCollection<PaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();

        protected Buyer()
        {
            _paymentMethods = new List<PaymentMethod>();
        }

        public Buyer(string email, string firstName, string lastName) : this()
        {
            Email = email ?? throw new OrderingDomainException(nameof(email));
            FirstName = firstName ?? throw new OrderingDomainException(nameof(firstName));
            LastName = lastName ?? throw new OrderingDomainException(nameof(lastName));
        }

        public PaymentMethod AddPaymentMethod(int cartTypeId, string alias, string cardNumber, string securityNumber,
            string cardHolderName, DateTime expiration, Guid orderId)
        {
            var existingPaymentMethod =
                _paymentMethods.FirstOrDefault(p => p.IsEqualsTo(cartTypeId, cardNumber, expiration));
            if (existingPaymentMethod is not null)
            {
                AddDomainEvent(new BuyerAndPaymentMethodVerifiedDomainEvent(this, existingPaymentMethod, orderId));
                return existingPaymentMethod;
            }

            var paymentMethod =
                new PaymentMethod(alias, cardNumber, securityNumber, cardHolderName, expiration, cartTypeId);
            _paymentMethods.Add(paymentMethod);
            AddDomainEvent(new BuyerAndPaymentMethodVerifiedDomainEvent(this, paymentMethod, orderId));
            return paymentMethod;
        }

        public override bool Equals(object obj)
        {
            return obj is Buyer buyer &&
                   Email == buyer.Email &&
                   FirstName == buyer.FirstName &&
                   LastName == buyer.LastName;
        }
    }
}