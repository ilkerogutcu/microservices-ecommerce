using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;

namespace Catalog.Infrastructure.Repositories
{
    public class OptionRepository : MongoDbRepositoryBase<Option>, IOptionRepository
    {
        public OptionRepository(ICatalogContext<Option> context) : base(context)
        {
        }
    }
}