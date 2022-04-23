using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class OptionRepository : MongoDbRepositoryBase<Option>, IOptionRepository
    {
        public OptionRepository(ICatalogContext context) : base(context)
        {
        }
    }
}