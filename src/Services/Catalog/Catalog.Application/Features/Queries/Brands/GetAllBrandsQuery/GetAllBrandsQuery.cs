using System.Collections.Generic;
using Catalog.Application.Dtos;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.Brands.GetAllBrandsQuery
{
    public class GetAllBrandsQuery : IRequest<IDataResult<List<BrandDto>>>
    {

        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}