using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Grpc.Entities;
using Catalog.Grpc.Interfaces.Repositories;
using Catalog.Grpc.Persistence;
using MongoDB.Driver;

namespace Catalog.Grpc.Repositories
{
    public class OptionValueRepository : MongoDbRepositoryBase<OptionValue>, IOptionValueRepository
    {
        public OptionValueRepository(ICatalogContext<OptionValue> context) : base(context)
        {
        }
    }
}