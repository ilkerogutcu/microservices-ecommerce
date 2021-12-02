using System.Collections.Generic;
using System.Text.Json.Serialization;
using Catalog.Domain.Entities;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Products.UpdateProductCommand
{
    public class UpdateProductCommand : IRequest<IDataResult<Product>>
    {
        [JsonIgnore]
        public string ProductId { get; set; }
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
        public bool IsFreeShipping { get; set; }
        public List<string> OptionValueIds { get; set; }
        public List<string> ImageUrls { get; set; }

    }
}