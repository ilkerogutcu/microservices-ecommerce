using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Application.Features.Commands.Products.AddCommentToProductCommand;
using Catalog.Application.Features.Commands.Products.CreateManyProductsCommand;
using Catalog.Application.Features.Commands.Products.UpdateProductActivationCommand;
using Catalog.Application.Features.Commands.Products.UpdateProductApprovalCommand;
using Catalog.Application.Features.Commands.Products.UpdateProductCommand;
using Catalog.Application.Features.Commands.Products.UpdateProductLockStatusCommand;
using Catalog.Application.Features.Queries.Products.GetAllProductsQuery;
using Catalog.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/products")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST api/v1/[controller]/
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Product>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost]
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateProduct(CreateManyProductsCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        // POST api/v1/[controller]/{productId}/comment
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost("{productId}/comment")]
        //[Authorize(Roles = "Buyer")]
        public async Task<IActionResult> AddCommentToProduct([FromRoute] string productId,
            [FromBody] AddCommentToProductCommand command)
        {
            command.ProductId = productId;
            var result = await _mediator.Send(command);
            return result.Success ? Ok() : BadRequest(result.Message);
        }


        // PUT api/v1/[controller]/{productId}
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut("{productId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateProduct([FromRoute] string productId, UpdateProductCommand command)
        {
            command.ProductId = productId;
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        // PATCH api/v1/[controller]/{productId}/activation
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPatch("{productId}/activation")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateProductActivation([FromRoute] string productId,
            UpdateProductActivationCommand command)
        {
            command.Id = productId;
            var result = await _mediator.Send(command);
            return result.Success ? Ok() : BadRequest(result.Message);
        }

        // PATCH api/v1/[controller]/{productId}/approval
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPatch("{productId}/approval")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateProductApproval([FromRoute] string productId,
            UpdateProductApprovalCommand command)
        {
            command.Id = productId;
            var result = await _mediator.Send(command);
            return result.Success ? Ok() : BadRequest(result.Message);
        }

        // PATCH api/v1/[controller]/{productId}/lock-status
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPatch("{productId}/lock-status")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateProductLockStatus([FromRoute] string productId,
            UpdateProductLockStatusCommand command)
        {
            command.Id = productId;
            var result = await _mediator.Send(command);
            return result.Success ? Ok() : BadRequest(result.Message);
        }

        // PATCH api/v1/[controller]/{productId}/lock-status
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetAllProducts([FromQuery] GetAllProductsQuery query)
        {
            var result = await _mediator.Send(query);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
    }
}