using System;

namespace Order.Application.Dtos
{
    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Units { get; set; }
        public decimal UnitPrice { get; set; }
        public string PictureUrl { get; set; }
    }
}