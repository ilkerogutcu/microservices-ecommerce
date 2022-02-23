using System.Threading.Tasks;
using Order.Application.Dtos;

namespace Order.Application.Interfaces.Services
{
    public interface ICatalogService
    {
        Task<ProductDetailsDto> GetProductDetailsByIdAsync(string id);
        Task<bool> UpdateProductStockQuantityById(string id, int quantity);
    }
}