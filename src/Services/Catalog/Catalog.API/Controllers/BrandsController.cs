using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Application.Features.Commands.Brands.CreateBrandCommand;
using Catalog.Application.Features.Commands.Brands.DeleteBrandCommand;
using Catalog.Application.Features.Commands.Brands.UpdateBrandCommand;
using Catalog.Application.Features.Queries.Brands.GetActiveBrandsQuery;
using Catalog.Application.Features.Queries.Brands.GetAllBrandsQuery;
using Catalog.Application.Features.Queries.Brands.GetBrandByIdQuery;
using Catalog.Application.Features.Queries.Brands.GetNotActiveBrandsQuery;
using Catalog.Application.Wrappers;
using Catalog.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/brands")]
    public class BrandController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BrandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/v1/[controller]?pageSize=10&pageIndex=1&isActive=null
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResult<List<BrandDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0,
            [FromQuery] bool? isActive = null)
        {
            switch (isActive)
            {
                case null:
                {
                    var result = await _mediator.Send(new GetAllBrandsQuery
                    {
                        PageIndex = pageIndex,
                        PageSize = pageSize
                    });
                    return result.Success ? Ok(result) : BadRequest(result.Message);
                }
                case false:
                {
                    var result = await _mediator.Send(new GetNotActiveBrandsQuery
                    {
                        PageIndex = pageIndex,
                        PageSize = pageSize
                    });
                    return result.Success ? Ok(result.Data) : BadRequest(result.Message);
                }
                default:
                {
                    var result = await _mediator.Send(new GetActiveBrandsQuery
                    {
                        PageIndex = pageIndex,
                        PageSize = pageSize
                    });
                    return result.Success ? Ok(result) : BadRequest(result.Message);
                }
            }
        }

        // GET api/v1/[controller]/{id}
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _mediator.Send(new GetBrandByIdQuery
            {
                Id = id
            });
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        // PUT api/v1/[controller]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Brand))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [Authorize(Roles = "Administrator")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBrandCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }


        // POST api/v1/[controller]/
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Brand))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBrandCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }


        // DELETE api/v1/[controller]/{id}
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.Send(new DeleteBrandCommand
            {
                Id = id
            });
            return result.Success ? Ok() : BadRequest(result.Message);
        }
    }
}