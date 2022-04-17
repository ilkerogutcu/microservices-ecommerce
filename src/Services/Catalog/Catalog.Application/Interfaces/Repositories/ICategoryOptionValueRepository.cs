using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Application.Features.Queries.Categories.GetCategoryOptionValuesByIdQuery;
using Catalog.Domain.Common;
using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces.Repositories
{
    public interface ICategoryOptionValueRepository: IDocumentDbRepository<CategoryOptionValue>
    {
       Task<GetCategoryOptionValuesByIdViewModel>  GetCategoryOptionValuesByIdAsync(string id);
    }
}