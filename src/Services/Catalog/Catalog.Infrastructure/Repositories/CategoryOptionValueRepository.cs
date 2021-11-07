using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;

namespace Catalog.Infrastructure.Repositories
{
    public class CategoryOptionValueRepository : MongoDbRepositoryBase<CategoryOptionValue>, ICategoryOptionValueRepository
    {
        public CategoryOptionValueRepository(ICatalogContext<CategoryOptionValue> context) : base(context)
        {
        }
    }
}