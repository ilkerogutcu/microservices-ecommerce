using Catalog.Grpc.Common;
using MongoDB.Driver;

namespace Catalog.Grpc.Persistence
{
    public interface ICatalogContext<T> where T : BaseEntity
    {
        IMongoCollection<T> GetCollection();
    }
}