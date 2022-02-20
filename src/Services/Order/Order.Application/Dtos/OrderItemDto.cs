namespace Order.Application.Dtos
{
    public class OrderItemDto
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Units { get; set; }
        public decimal UnitPrice { get; set; }
        public string PictureUrl { get; set; }
    }
}