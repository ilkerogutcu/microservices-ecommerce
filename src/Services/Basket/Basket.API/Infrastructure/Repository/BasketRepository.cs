using System.Threading.Tasks;
using Basket.API.Core.Application.Repository;
using Basket.API.Core.Domain.Models;
using Newtonsoft.Json;
using Serilog;
using StackExchange.Redis;

namespace Basket.API.Infrastructure.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public BasketRepository(ConnectionMultiplexer redis)
        {
            _redis = redis;
            _database = redis.GetDatabase();
        }

        public Task<CustomerBasket> GetBasketAsync(string customerId)
        {
            var data = _database.StringGet(customerId);
            return Task.FromResult(data.IsNullOrEmpty ? null : JsonConvert.DeserializeObject<CustomerBasket>(data));
        }

        public Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            Log.Information("Updating Basket for customer {@CustomerId}", basket.BuyerId);
            var created = _database.StringSet(basket.BuyerId, JsonConvert.SerializeObject(basket));
            if (!created)
            {
                Log.Warning("Basket not updated for customer {@CustomerId}", basket.BuyerId);
                return null;
            }

            Log.Information("Basket updated for customer {@CustomerId}", basket.BuyerId);
            return Task.FromResult(basket);
        }

        public Task<bool> DeleteBasketAsync(string id)
        {
            Log.Information("Deleting Basket for customer {@CustomerId}", id);
            var deleted = _database.KeyDelete(id);
            if (!deleted)
            {
                Log.Warning("Basket not deleted for customer {@CustomerId}", id);
                return Task.FromResult(deleted);
            }

            Log.Information("Basket deleted for customer {@CustomerId}", id);
            return Task.FromResult(deleted);
        }
    }
}