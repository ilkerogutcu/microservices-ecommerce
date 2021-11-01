using System.Collections.Generic;
using Catalog.Domain.Common;

namespace Catalog.Domain.Entities
{
    public class Sku : BaseEntity
    {
        public string Barcode { get; set; }
        public string StockCode { get; set; }
        public int StockQuantity { get; set; }

        public decimal SalePrice { get; set; }
        public decimal? ListPrice { get; set; }
        public bool IsFreeShipping => SalePrice >= 150;
        public bool Approved { get; set; }
        public bool Locked { get; set; }    
        public IList<OptionValue> OptionValues { get; set; } = new List<OptionValue>();

        public void AddOptionValue(OptionValue optionValue)
        {
            OptionValues.Add(optionValue);
        }
    }
}