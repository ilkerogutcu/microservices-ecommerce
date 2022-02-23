using Catalog.Grpc.Entities;
using Catalog.Grpc.Interfaces.Repositories;
using Catalog.Grpc.Persistence;

namespace Catalog.Grpc.Repositories
{
    public class CategoryRepository : MongoDbRepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(ICatalogContext<Category> context) : base(context)
        {
        }
    }
}