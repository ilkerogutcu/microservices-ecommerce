using Catalog.Application.Dtos;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.Brands.GetBrandByIdQuery
{
    public class GetBrandByIdQuery : IRequest<IDataResult<BrandDto>>
    {
        public GetBrandByIdQuery(string id)
        {
            Id = id;
        }
        public string Id { get; }
    }
}