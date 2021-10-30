using Catalog.Domain.Common;
using Catalog.Domain.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Persistence
{
    public interface ICatalogContext<T> where T : BaseEntity
    {
        IMongoCollection<T> GetCollection();
    }
}