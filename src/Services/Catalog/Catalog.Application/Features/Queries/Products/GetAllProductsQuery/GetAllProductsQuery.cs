using System.Collections.Generic;
using Catalog.Application.Dtos;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.Products.GetAllProductsQuery
{
    public class GetAllProductsQuery : IRequest<IDataResult<List<ProductDto>>>
    {
        public int PageSize { get; set; } = 10;

        public int PageIndex { get; set; } = 0;

        // Timestamp start date
        public long StartDate { get; set; }

        // Timestamp end date
        public long EndDate { get; set; }
        public string ProductName { get; set; }

        public string ModelCode { get; set; }
        public bool IsActive { get; set; } = true;
        public string Barcode { get; set; }
        public string StockCode { get; set; }
        public bool Approved { get; set; } = true;
        public bool Locked { get; set; } = false;
    }
}