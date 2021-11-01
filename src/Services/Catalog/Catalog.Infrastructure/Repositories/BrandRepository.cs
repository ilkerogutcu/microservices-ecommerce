using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;

namespace Catalog.Infrastructure.Repositories
{
    public class BrandRepository: MongoDbRepositoryBase<Brand>, IBrandRepository
    {
        public BrandRepository(ICatalogContext<Brand> context) : base(context)
        {
        }
    }
}