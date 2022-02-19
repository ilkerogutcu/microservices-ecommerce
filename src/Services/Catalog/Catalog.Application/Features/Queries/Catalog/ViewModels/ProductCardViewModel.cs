namespace Catalog.Application.Features.Queries.Catalog.ViewModels
{
    public class ProductCardViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string BrandId { get; set; }
        public string ThumbnailImageUrl { get; set; }
        public string ShortDescription { get; set; }
        public int ReviewsCount { get; set; }
        public double? RatingAverage { get; set; }
        public string Barcode { get; set; }
        public int StockQuantity { get; set; }
        public decimal SalePrice { get; set; }
        public decimal? ListPrice { get; set; }
        public bool IsFreeShipping { get; set; }
        public int DiscountRate { get; set; }
    }
}