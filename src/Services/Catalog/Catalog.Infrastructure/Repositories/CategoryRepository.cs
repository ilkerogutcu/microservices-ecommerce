using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;

namespace Catalog.Infrastructure.Repositories
{
    public class CategoryRepository: MongoDbRepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(ICatalogContext<Category> context) : base(context)
        {
        }
    }
}