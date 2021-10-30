using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Application.Features.Commands.CreateBrandCommand;
using Catalog.Application.Features.Commands.UpdateBrandCommand;
using Catalog.Application.Features.Queries.Brands.GetActiveBrandsQuery;
using Catalog.Application.Features.Queries.Brands.GetAllBrandsQuery;
using Catalog.Application.Features.Queries.Brands.GetBrandByIdQuery;
using Catalog.Application.Features.Queries.Brands.GetNotActiveBrandsQuery;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BrandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/v1/[controller]?pageSize=10&pageIndex=1&isActive=null
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<BrandDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0,
            [FromQuery] bool? isActive = null)
        {
            switch (isActive)
            {
                case null:
                {
                    var result = await _mediator.Send(new GetAllBrandsQuery(pageSize, pageIndex));
                    return result.Success ? Ok(result.Data) : BadRequest(result.Message);
                }
                case true:
                {
                    var result = await _mediator.Send(new GetActiveBrandsQuery(pageSize, pageIndex));
                    return result.Success ? Ok(result.Data) : BadRequest(result.Message);
                }
                default:
                {
                    var result = await _mediator.Send(new GetNotActiveBrandsQuery(pageSize, pageIndex));
                    return result.Success ? Ok(result.Data) : BadRequest(result.Message);
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
            var result = await _mediator.Send(new GetBrandByIdQuery(id));
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }
        
        // PUT api/v1/[controller]/{id}
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] BrandDto brandDto)
        {
            var result = await _mediator.Send(new UpdateBrandCommand(brandDto));
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }
        
        
        // POST api/v1/[controller]/
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBrandCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
    }
}