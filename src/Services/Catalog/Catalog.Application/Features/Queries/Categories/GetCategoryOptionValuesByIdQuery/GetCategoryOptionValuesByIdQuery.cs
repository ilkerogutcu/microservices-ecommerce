using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.Categories.GetCategoryOptionValuesByIdQuery
{
    public class GetCategoryOptionValuesByIdQuery : IRequest<IDataResult<GetCategoryOptionValuesByIdViewModel>>
    {
        public string CategoryId { get; set; }
    }
}