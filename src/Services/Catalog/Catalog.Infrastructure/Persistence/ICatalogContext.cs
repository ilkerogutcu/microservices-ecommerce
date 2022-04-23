using Catalog.Domain.Common;
using Catalog.Domain.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Persistence
{
    public interface ICatalogContext
    {
        IMongoCollection<T> GetCollection<T>();
    }
}