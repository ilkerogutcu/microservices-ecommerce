using System.Collections.Generic;

namespace Catalog.Application.Dtos
{
    public class CreateProductDto
    {
        public string CategoryId { get; set; }
        public string BrandId { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string ModelCode { get; set; }
        public string Barcode { get; set; }
        public string StockCode { get; set; }
        public int StockQuantity { get; set; }
        public decimal SalePrice { get; set; }
        public decimal? ListPrice { get; set; }
        public List<string> OptionValueIds { get; set; }
        
        public List<string> ImageUrls { get; set; }
    }
}