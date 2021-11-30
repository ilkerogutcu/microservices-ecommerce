using System.Collections.Generic;

namespace Catalog.Application.Dtos
{
    public class SkuDto
    {
        public string Barcode { get; set; }
        public string StockCode { get; set; }
        public int StockQuantity { get; set; }
        public decimal SalePrice { get; set; }
        public decimal? ListPrice { get; set; }
        public IList<OptionValueDto> OptionValues { get; set; } = new List<OptionValueDto>();
    }
}