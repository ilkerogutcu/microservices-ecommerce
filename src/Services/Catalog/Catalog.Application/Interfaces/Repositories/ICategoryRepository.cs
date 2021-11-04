using Catalog.Domain.Common;
using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces.Repositories
{
    public interface ICategoryRepository: IDocumentDbRepository<Category>
    {
        
    }
}