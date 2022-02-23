using System.Threading.Tasks;
using Catalog.Grpc.Entities;
using Catalog.Grpc.ViewModels;

namespace Catalog.Grpc.Interfaces.Repositories
{
    public interface IProductRepository : IDocumentDbRepository<Product>
    {
        Task<ProductDetailsViewModel> GetProductDetailsByIdAsync(string id);
    }
}