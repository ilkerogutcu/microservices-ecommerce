using System.Threading.Tasks;
using Basket.API.Core.Domain.Models;

namespace Basket.API.Core.Application.Services
{
    public interface IBasketService
    {
        Task<CustomerBasket> GetBasketAsync();
        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
        Task<bool> DeleteBasketAsync();
        Task<bool> AddItemToBasketAsync(BasketItem basketItem);
        Task<bool> CheckoutAsync(BasketCheckout basketCheckout);
    }
}