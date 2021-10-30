using System.Collections.Generic;
using Catalog.Application.Dtos;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.Brands.GetActiveBrandsQuery
{
    public class GetActiveBrandsQuery : IRequest<IDataResult<List<BrandDto>>>
    {
        public GetActiveBrandsQuery(int pageSize, int pageIndex)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
        }

        public int PageSize { get; }
        public int PageIndex { get; }
    }
}