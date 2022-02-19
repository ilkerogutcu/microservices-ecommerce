using System.Threading.Tasks;
using Basket.API.Core.Application.Services;
using Basket.API.Core.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/basket")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerBasket))]
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketByIdAsync()
        {
            var basket = await _basketService.GetBasketAsync();
            return Ok(basket);
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerBasket))]
        [HttpPut]
        public async Task<ActionResult<CustomerBasket>> UpdateBasketAsync([FromBody] CustomerBasket basket)
        {
            var result = await _basketService.UpdateBasketAsync(basket);
            return Ok(result);
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult> AddItemToBasket([FromBody] BasketItem basketItem)
        {
            var result = await _basketService.AddItemToBasketAsync(basketItem);
            return result ? Ok() : BadRequest();
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete]
        public async Task<ActionResult> DeleteBasketById()
        {
            var result = await _basketService.DeleteBasketAsync();
            return result ? Ok() : BadRequest();
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("checkout")]
        public async Task<ActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var result = await _basketService.CheckoutAsync(basketCheckout);
            return result ? Ok() : BadRequest();
        }
    }
}