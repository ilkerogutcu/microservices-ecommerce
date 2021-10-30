using Catalog.Domain.Common;
using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces
{
    public interface IProductRepository : IDocumentDbRepository<Product>
    {
        
    }
}