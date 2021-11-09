using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Application.Features.Commands.Categories.CreateCategoryCommand;
using Catalog.Application.Features.Commands.Categories.CreateOrUpdateCategoryOptionValueCommand;
using Catalog.Application.Features.Commands.Categories.DeleteCategoryCommand;
using Catalog.Application.Features.Commands.Categories.UpdateCategoryCommand;
using Catalog.Application.Features.Queries.Categories.GetAllCategoriesQuery;
using Catalog.Application.Features.Queries.Categories.GetCategoryByIdQuery;
using Catalog.Application.Features.Queries.Categories.GetCategoryOptionValuesByIdQuery;
using Catalog.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST api/v1/[controller]/
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        // POST api/v1/[controller]/categoryId/optionValues
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryOptionValue))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost("optionValues")]
        public async Task<IActionResult> CreateOrUpdateCategoryOptionValue([FromBody] CreateOrUpdateCategoryOptionValueCommand command)
        {
            
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }
        
        // GET api/v1/[controller]/categoryId/OptionValues
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryOptionValueDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("{categoryId}/optionValues")]
        public async Task<IActionResult> GetCategoryOptionValues(string categoryId)
        {
            
            var result = await _mediator.Send(new GetCategoryOptionValuesByIdQuery()
            {
                Id = categoryId
            });
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }


        // PUT api/v1/[controller]/
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        // DELETE api/v1/[controller]/
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok() : BadRequest(result.Message);
        }

        // GET api/v1/[controller]?pageSize=null&pageIndex=null&isActive=null
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoryDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool? isActive = null)
        {
            var result = await _mediator.Send(new GetAllCategoriesQuery
            {
                IsActive = isActive
            });
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        // GET api/v1/[controller]/{id}
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery()
            {
                Id = id
            });
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }
    }
}