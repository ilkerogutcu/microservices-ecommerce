using System;
using System.Collections.Generic;
using Order.Domain.AggregateModels.BuyerAggregate;
using Order.Domain.Events;
using Order.Domain.Exceptions;
using Order.Domain.SeedWork;

namespace Order.Domain.AggregateModels.OrderAggregate
{
    public class Order : BaseEntity, IAggregateRoot
    {
        public DateTime OrderDate { get; set; }
        public Guid? BuyerId { get; set; }
        public Buyer Buyer { get; set; }
        public Address Address { get; set; }
        public string PaymentHtmlContent { get; set; }
        public int orderStatusId;
        public string PaymentId { get; set; }
        public OrderStatus OrderStatus { get;  set; }
        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public Order()
        {
            Id = Guid.NewGuid();
            _orderItems = new List<OrderItem>();
            CreatedDate = DateTime.Now;
        }

        public Order(string email, string firstName, string lastName, Guid userId, Address address, int cartTypeId, string cardNumber,
            string cardSecurityNumber, string cardHolderName, string cardExpirationMonth, string cardExpirationYear, Guid? buyerId) : this()
        {
            BuyerId = buyerId;
            orderStatusId = OrderStatus.WaitingForPayment.Id;
            OrderDate = DateTime.UtcNow;
            Address = address;
            AddOrderStartedDomainEvent(email, firstName, lastName, userId, cartTypeId, cardNumber, cardSecurityNumber, cardHolderName,
                cardExpirationMonth,cardExpirationYear);
        }

        private void AddOrderStartedDomainEvent(string email, string firstName, string lastName, Guid userId, int cartTypeId, string cardNumber,
            string cardSecurityNumber, string cardHolderName, string cardExpirationMonth, string cardExpirationYear)
        {
            var orderStartedDomainEvent =
                new OrderStartedDomainEvent(email, cartTypeId, cardNumber, cardHolderName, cardSecurityNumber, cardExpirationMonth, cardExpirationYear,this, userId,
                    firstName, lastName);
            AddDomainEvent(orderStartedDomainEvent);
        }

        public void AddOrderItem(string productId, string productName, string pictureUrl, decimal unitPrice, int units)
        {
            if (string.IsNullOrEmpty(productId))
            {
                throw new OrderingDomainException(nameof(productId));
            }

            if (string.IsNullOrEmpty(productName))
            {
                throw new OrderingDomainException(nameof(productName));
            }

            if (string.IsNullOrEmpty(pictureUrl))
            {
                throw new OrderingDomainException(nameof(pictureUrl));
            }

            if (unitPrice < 0)
            {
                throw new OrderingDomainException(nameof(unitPrice));
            }

            if (units < 1)
            {
                throw new OrderingDomainException(nameof(units));
            }

            var existingOrderItem = _orderItems.Find(o => o.ProductId == productId);
            if (existingOrderItem != null)
            {
                existingOrderItem.Units += units;
                return;
            }

            var orderItem = new OrderItem(productId, productName, pictureUrl, unitPrice, units);
            _orderItems.Add(orderItem);
        }

        public void SetBuyerId(Guid buyerId)
        {
            BuyerId = buyerId;
        }
    }
}