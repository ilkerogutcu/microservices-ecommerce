using Catalog.Application.Dtos;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.Categories.GetCategoryByIdQuery
{
    public class GetCategoryByIdQuery : IRequest<IDataResult<CategoryDto>>
    {
        public string Id { get; set; }
    }
}