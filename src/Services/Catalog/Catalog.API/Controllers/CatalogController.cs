using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Application.Features.Queries.Catalog.GetCommentsByProductIdQuery;
using Catalog.Application.Features.Queries.Catalog.GetProductDetailsByIdQuery;
using Catalog.Application.Features.Queries.Catalog.GetProductsByCategoryIdQuery;
using Catalog.Application.Features.Queries.Catalog.GetTopProductsQuery;
using Catalog.Application.Features.Queries.Catalog.ViewModels;
using Catalog.Application.Wrappers;
using Catalog.Domain.Enums;
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
        public async Task<ActionResult<IEnumerable<ProductCardViewModel>>> GetTopProducts()
        {
            var result = await _mediator.Send(new GetTopProductsQuery());
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResult<List<ProductCardViewModel>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("category/{categoryId}/products")]
        public async Task<ActionResult<IEnumerable<ProductCardViewModel>>> GetProductsByCategoryId([FromRoute] string categoryId,
            [FromQuery] SortBy sortBy, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var result = await _mediator.Send(new GetProductsByCategoryIdQuery(categoryId, sortBy, pageSize, pageIndex));
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductDetailsViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("product/{productId}")]
        public async Task<ActionResult<IEnumerable<ProductCardViewModel>>> GetProductDetailsById([FromRoute] string productId)
        {
            var result = await _mediator.Send(new GetProductDetailsByIdQuery(productId));
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CommentDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("product/{productId}/comments")]
        public async Task<ActionResult<IEnumerable<ProductCardViewModel>>> GetCommentsByProductId([FromRoute] string productId)
        {
            var result = await _mediator.Send(new GetCommentsByProductIdQuery(productId));
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }
    }
}