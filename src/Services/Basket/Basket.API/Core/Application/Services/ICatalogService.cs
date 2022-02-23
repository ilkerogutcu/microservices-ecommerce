using System.Threading.Tasks;
using Basket.API.Core.Application.Dtos;

namespace Basket.API.Core.Application.Services
{
    public interface ICatalogService
    {
        Task<ProductDetailsDto> GetProductDetailsByIdAsync(string id);
        Task<bool> UpdateProductStockQuantityById(string id, int quantity);
    }
}