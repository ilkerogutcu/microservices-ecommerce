using System;
using System.Collections.Generic;
using Order.Application.Dtos;
using Order.Domain.AggregateModels.OrderAggregate;

namespace Order.Application.Features.Queries.ViewModels
{
    public class OrderDetailViewModel
    {
        public string OrderNumber { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Zip { get; set; }
        public string AddressLine { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public decimal Total { get; set; }
    }
}