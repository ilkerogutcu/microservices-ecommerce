using System;
using System.Linq;
using System.Threading.Tasks;
using Basket.API.Constants;
using Basket.API.Core.Application.Repository;
using Basket.API.Core.Domain.Models;
using Basket.API.IntegrationEvents.Events;
using EventBus.Base.Abstraction;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;

namespace Basket.API.Core.Application.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IIdentityService _identityService;
        private readonly IEventBus _eventBus;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICatalogService _catalogService;

        public BasketService(IHttpContextAccessor httpContextAccessor, IIdentityService identityService,
            IBasketRepository basketRepository, IEventBus eventBus, ICatalogService catalogService)
        {
            _httpContextAccessor = httpContextAccessor;
            _identityService = identityService;
            _basketRepository = basketRepository;
            _eventBus = eventBus;
            _catalogService = catalogService;
        }

        public async Task<CustomerBasket> GetBasketAsync()
        {
            try
            {
                Log.Information("GetBasketAsync called");
                var userId = await _identityService.GetUserIdAsync();
                var customerBasketFromCookie = GetCustomerBasketFromCookie();

                if (userId == default)
                {
                    return customerBasketFromCookie ?? new CustomerBasket();
                }

                var customerBasketFromRedis = await _basketRepository.GetBasketAsync(userId);

                if (customerBasketFromCookie is null) return customerBasketFromRedis ?? new CustomerBasket(userId);
                foreach (var basketItem in customerBasketFromCookie.Items.Where(basketItem =>
                             !customerBasketFromRedis.Items.Contains(basketItem)))
                {
                    customerBasketFromRedis.Items.Add(basketItem);
                }

                return customerBasketFromRedis ?? new CustomerBasket(userId);
            }
            catch (Exception e)
            {
                Log.Error(e, "GetBasketAsync failed");
                return null;
            }
        }

        private CustomerBasket GetCustomerBasketFromCookie()
        {
            var basketFromCookie = _httpContextAccessor.HttpContext?.Request.Cookies[CookieNames.BasketItems];
            if (string.IsNullOrEmpty(basketFromCookie))
            {
                return null;
            }

            var customerBasket = JsonConvert.DeserializeObject<CustomerBasket>(basketFromCookie);
            return customerBasket;
        }

        public Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            try
            {
                Log.Information("UpdateBasketAsync called");
                return _basketRepository.UpdateBasketAsync(basket);
            }
            catch (Exception e)
            {
                Log.Error(e, "UpdateBasketAsync failed");
                return null;
            }
        }

        public async Task<bool> DeleteBasketAsync(Guid userId)
        {
            try
            {
                Log.Information("DeleteBasketAsync called");

                if (userId != default) return await _basketRepository.DeleteBasketAsync(userId);
                _httpContextAccessor.HttpContext?.Response.Cookies.Delete(CookieNames.BasketItems);
                return true;
            }
            catch (Exception e)
            {
                Log.Error(e, "DeleteBasketAsync failed");
                return false;
            }
        }
        

        public async Task<bool> AddItemToBasketAsync(BasketItem basketItem)
        {
            try
            {
                Log.Information("AddItemToBasketAsync called");

                CustomerBasket customerBasket;
                var userId = await _identityService.GetUserIdAsync();

                if (userId == default)
                {
                    customerBasket = GetCustomerBasketFromCookie() ?? new CustomerBasket();
                    customerBasket.Items.Add(basketItem);
                    _httpContextAccessor.HttpContext?.Response.Cookies.Append(CookieNames.BasketItems,
                        JsonConvert.SerializeObject(customerBasket),
                        new CookieOptions
                        {
                            Expires = DateTime.Now.AddDays(1)
                        });
                }
                else
                {
                    customerBasket = await _basketRepository.GetBasketAsync(userId) ?? new CustomerBasket(userId);
                    customerBasket.Items.Add(basketItem);
                    await _basketRepository.UpdateBasketAsync(customerBasket);
                }

                return true;
            }
            catch (Exception e)
            {
                Log.Error(e, "AddItemToBasketAsync failed");
                return false;
            }
        }

        public async Task<bool> CheckoutAsync(BasketCheckout basketCheckout)
        {
            try
            {
                Log.Information("CheckoutAsync called");
                var userId = await _identityService.GetUserIdAsync();
                if (userId == default)
                {
                    Log.Warning("CheckoutAsync failed, userId is null");
                    return false;
                }

                var customerBasket = await _basketRepository.GetBasketAsync(userId);
                if (customerBasket is null)
                {
                    Log.Warning("CheckoutAsync failed, customerBasket is null");
                    return false;
                }

                foreach (var basketItem in customerBasket.Items)
                {
                    var product = await _catalogService.GetProductDetailsByIdAsync(basketItem.ProductId);
                    if (product is null || product.StockQuantity <= 0)
                    {
                        customerBasket.Items.Remove(basketItem);
                        continue;
                    }

                    basketItem.ProductId = product.Id;
                    basketItem.BrandName = product.Brand;
                    basketItem.ProductName = product.Name;
                    basketItem.UnitPrice = product.SalePrice;
                    basketItem.Color = product.Color;
                    basketItem.HexCode = product.HexCode;
                    basketItem.Size = product.Size;
                    if (basketItem.Quantity <= product.StockQuantity) continue;
                    Log.Warning("CheckoutAsync failed, product stock is not enough");
                    return false;
                }

                var eventMessage = new OrderCreatedIntegrationEvent(userId, basketCheckout.City, basketCheckout.District, basketCheckout.Zip,
                    basketCheckout.FirstName, basketCheckout.LastName, basketCheckout.PhoneNumber, basketCheckout.AddressLine,
                    basketCheckout.AddressTitle, basketCheckout.CardNumber, basketCheckout.CardHolderName, basketCheckout.CardExpirationMonth,
                    basketCheckout.CardExpirationYear, basketCheckout.CardSecurityNumber, basketCheckout.CardTypeId, basketCheckout.Buyer,
                    customerBasket, basketCheckout.Email);

                _eventBus.Publish(eventMessage);
                return true;
            }
            catch (Exception e)
            {
                Log.Error(e, "CheckoutAsync failed");
                return false;
            }
        }
    }
}