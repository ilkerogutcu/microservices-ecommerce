using System.Collections.Generic;
using Catalog.Application.Dtos;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.Brands.GetBrandsByIsActiveQuery
{
    public class GetBrandsByIsActiveQuery : IRequest<IDataResult<List<BrandDto>>>
    {
        public GetBrandsByIsActiveQuery(int pageSize, int pageIndex)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
        }

        public int PageSize { get; }
        public int PageIndex { get; }
    }
}