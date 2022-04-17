namespace Order.Application.Dtos
{
    public class ProductDetailsDto
    {
        public string Id { get; set; }
        public string BrandId { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string ModelCode { get; set; }
        public string Barcode { get; set; }
        public decimal SalePrice { get; set; }
        public string Color { get; set; }
        public string HexCode { get; set; }
        public string Size { get; set; }
        public int StockQuantity { get; set; }
    }
}