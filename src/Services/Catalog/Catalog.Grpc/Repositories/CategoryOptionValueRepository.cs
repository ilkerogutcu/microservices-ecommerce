using System.Threading.Tasks;
using Catalog.Grpc.Entities;
using Catalog.Grpc.Interfaces.Repositories;
using Catalog.Grpc.Persistence;

namespace Catalog.Grpc.Repositories
{
    public class CategoryOptionValueRepository : MongoDbRepositoryBase<CategoryOptionValue>,
        ICategoryOptionValueRepository
    {
        public CategoryOptionValueRepository(ICatalogContext<CategoryOptionValue> context) : base(context)
        {
        }
        
    }
}