using System;
using System.Threading.Tasks;
using Basket.API.Core.Domain.Models;

namespace Basket.API.Core.Application.Repository
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(Guid customerId);
        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
        Task<bool> DeleteBasketAsync(Guid id);
    }
}