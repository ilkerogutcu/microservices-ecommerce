namespace Order.Domain.Models
{
    public class BasketItem 
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string BrandName { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal UnitPrice { get; set; }
        public string Color { get; set; }
        public string HexCode { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
    }
}