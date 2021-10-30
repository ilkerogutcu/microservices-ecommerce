using System.Collections.Generic;
using Catalog.Application.Dtos;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.Brands.GetNotActiveBrandsQuery
{
    public class GetNotActiveBrandsQuery : IRequest<IDataResult<List<BrandDto>>>
    {
        public GetNotActiveBrandsQuery(int pageSize, int pageIndex)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
        }

        public int PageSize { get; }
        public int PageIndex { get; }
    }
}