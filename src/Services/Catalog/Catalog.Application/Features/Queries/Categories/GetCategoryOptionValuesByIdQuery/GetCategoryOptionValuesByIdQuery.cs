using Catalog.Application.Dtos;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.Categories.GetCategoryOptionValuesByIdQuery
{
    public class GetCategoryOptionValuesByIdQuery : IRequest<IDataResult<CategoryOptionValueDto>>
    {
        public string Id { get; set; }
    }
}