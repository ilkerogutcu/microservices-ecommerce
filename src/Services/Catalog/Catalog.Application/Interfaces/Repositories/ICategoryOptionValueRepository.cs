using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Domain.Common;
using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces.Repositories
{
    public interface ICategoryOptionValueRepository: IDocumentDbRepository<CategoryOptionValue>
    {
    }
}