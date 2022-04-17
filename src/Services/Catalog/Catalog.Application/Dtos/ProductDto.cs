using System.Collections.Generic;

namespace Catalog.Application.Dtos
{
    public class ProductDto
    {
        public string ProductId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryId { get; set; }
        public string BrandName { get; set; }
        public string BrandId { get; set; }
        public string ThumbnailImageUrl { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string ModelCode { get; set; }
        public int ReviewsCount { get; set; }
        public double? RatingAverage { get; set; }
        public int RatingCount { get; set; }
        public bool IsActive { get; set; }
        public string Barcode { get; set; }
        public string StockCode { get; set; }
        public int StockQuantity { get; set; }
        public decimal SalePrice { get; set; }
        public decimal? ListPrice { get; set; }
        public bool IsFreeShipping { get; set; }
        public bool Approved { get; set; }
        public bool Locked { get; set; }
        public string Color { get; set; }
        public string HexCode { get; set; }
        public string Size { get; set; }
        public string CreatedBy { get; set; }
        public long CreatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public long? LastUpdatedTime { get; set; }
        public List<OptionValueDetailsDto> OptionValues { get; set; }
    }
}