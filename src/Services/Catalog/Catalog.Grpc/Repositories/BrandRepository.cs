using Catalog.Grpc.Entities;
using Catalog.Grpc.Interfaces.Repositories;
using Catalog.Grpc.Persistence;

namespace Catalog.Grpc.Repositories
{
    public class BrandRepository : MongoDbRepositoryBase<Brand>, IBrandRepository
    {
        public BrandRepository(ICatalogContext<Brand> context) : base(context)
        {
        }
    }
}