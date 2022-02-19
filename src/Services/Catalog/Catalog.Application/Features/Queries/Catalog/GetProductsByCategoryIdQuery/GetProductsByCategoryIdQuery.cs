using System.Collections.Generic;
using Catalog.Application.Features.Queries.Catalog.ViewModels;
using Catalog.Domain.Enums;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.Catalog.GetProductsByCategoryIdQuery
{
    public class GetProductsByCategoryIdQuery : IRequest<IDataResult<List<ProductCardViewModel>>>
    {
        public string CategoryId { get; }
        public SortBy SortBy { get; }
        public int PageSize { get; }
        public int PageIndex { get; }

        public GetProductsByCategoryIdQuery(string categoryId, SortBy sortBy = SortBy.MostRecent, int pageSize = 30, int pageIndex = 0)
        {
            CategoryId = categoryId;
            SortBy = sortBy;
            PageSize = pageSize;
            PageIndex = pageIndex;
        }
    }
}