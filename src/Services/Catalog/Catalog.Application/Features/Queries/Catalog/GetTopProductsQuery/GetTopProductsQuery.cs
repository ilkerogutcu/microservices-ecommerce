using System.Collections.Generic;
using Catalog.Application.Features.Queries.Catalog.ViewModels;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.Catalog.GetTopProductsQuery
{
    public class GetTopProductsQuery  : IRequest<IDataResult<List<ProductCardViewModel>>>
    {
        public int Count { get; set; } = 10;
    }
}