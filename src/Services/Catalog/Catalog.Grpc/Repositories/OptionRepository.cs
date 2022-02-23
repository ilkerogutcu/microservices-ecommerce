using Catalog.Grpc.Entities;
using Catalog.Grpc.Interfaces.Repositories;
using Catalog.Grpc.Persistence;

namespace Catalog.Grpc.Repositories
{
    public class OptionRepository : MongoDbRepositoryBase<Option>, IOptionRepository
    {
        public OptionRepository(ICatalogContext<Option> context) : base(context)
        {
        }
    }
}