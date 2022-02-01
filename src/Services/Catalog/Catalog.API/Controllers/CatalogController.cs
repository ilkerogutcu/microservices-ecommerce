using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Application.Features.Queries.Catalog.GetTopProductsQuery;
using Catalog.Application.Features.Queries.Catalog.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/catalog")]
    public class CatalogController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CatalogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductCardViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("top-products")]
        public async Task<ActionResult<IEnumerable<ProductCardViewModel>>> GetTopProducts(
            [FromQuery] GetTopProductsQuery query)
        {
            var result = await _mediator.Send(query);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }
    }
}