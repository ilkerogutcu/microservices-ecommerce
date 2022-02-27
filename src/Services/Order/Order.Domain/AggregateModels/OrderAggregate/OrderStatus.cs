using System;
using System.Collections.Generic;
using System.Linq;
using Order.Domain.Exceptions;
using Order.Domain.SeedWork;

namespace Order.Domain.AggregateModels.OrderAggregate
{
    public class OrderStatus : Enumeration
    {
        public static OrderStatus WaitingForPayment = new OrderStatus(1, nameof(WaitingForPayment).ToLowerInvariant());

        public static OrderStatus AwaitingValidation =
            new OrderStatus(2, nameof(AwaitingValidation).ToLowerInvariant());

        public static OrderStatus StockConfirmed = new OrderStatus(3, nameof(StockConfirmed).ToLowerInvariant());
        public static OrderStatus Paid = new OrderStatus(4, nameof(Paid).ToLowerInvariant());
        public static OrderStatus Shipped = new OrderStatus(5, nameof(Shipped).ToLowerInvariant());
        public static OrderStatus Cancelled = new OrderStatus(6, nameof(Cancelled).ToLowerInvariant());

        public OrderStatus(int id, string name) : base(id, name)
        {
        }

        public static IEnumerable<OrderStatus> List() => new[]
            {WaitingForPayment, AwaitingValidation, StockConfirmed, Paid, Shipped, Cancelled};

        public static OrderStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(x => string.Equals(x.Name, name, StringComparison.CurrentCultureIgnoreCase));
            return state ??
                   throw new OrderingDomainException(
                       $"Possible values for OrderStatus: {string.Join(",", List().Select(x => x.Name))}");
        }

        public static OrderStatus From(int id)
        {
            var state = List().SingleOrDefault(x => x.Id == id);
            return state ??
                   throw new OrderingDomainException(
                       $"Possible values for OrderStatus: {string.Join(",", List().Select(x => x.Name))}");
        }
    }
}