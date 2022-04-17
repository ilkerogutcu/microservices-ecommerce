using System.Collections.Generic;
using Catalog.Application.Dtos;

namespace Catalog.Application.Features.Queries.Catalog.ViewModels
{
    public class ProductDetailsViewModel
    {
        public string Id { get; set; }
        public string BrandId { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public string ModelCode { get; set; }
        public double? RatingAverage { get; set; }
        public int RatingCount { get; set; }
        public string LongDescription { get; set; }
        public string ShortDescription { get; set; }
        public string Barcode { get; set; }
        public decimal SalePrice { get; set; }
        public decimal? ListPrice { get; set; }
        public int DiscountRate { get; set; }
        public bool IsFreeShipping { get; set; }
        public string Color { get; set; }
        public string HexCode { get; set; }
        public string Size { get; set; }
        public int StockQuantity { get; set; }
        public List<string> ImageUrls { get; set; }
        public List<OptionValueDetailsDto> OptionValues { get; set; }
    }
}