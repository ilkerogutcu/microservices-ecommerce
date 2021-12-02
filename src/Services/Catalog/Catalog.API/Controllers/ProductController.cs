using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Application.Features.Commands.Products.CreateManyProductsCommand;
using Catalog.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
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
        public async Task<IActionResult> CreateProduct(CreateManyProductsCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
    }
}