using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Application.Features.Commands.Categories.CreateCategoryCommand;
using Catalog.Application.Features.Commands.Categories.CreateCategoryOptionValueCommand;
using Catalog.Application.Features.Commands.Categories.DeleteCategoryCommand;
using Catalog.Application.Features.Commands.Categories.DeleteCategoryOptionValueCommand;
using Catalog.Application.Features.Commands.Categories.UpdateCategoryCommand;
using Catalog.Application.Features.Queries.Categories.GetAllCategoriesQuery;
using Catalog.Application.Features.Queries.Categories.GetCategoryByIdQuery;
using Catalog.Application.Features.Queries.Categories.GetCategoryOptionValuesByIdQuery;
using Catalog.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/categories")]
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

        // POST api/v1/[controller]/{categoryId}/option-values
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryOptionValue))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost("{categoryId}/option-value")]
        public async Task<IActionResult> CreateCategoryOptionValue(string categoryId,
            [FromBody] CreateCategoryOptionValueCommand request)
        {
            var result = await _mediator.Send(new CreateCategoryOptionValueCommand
            {
                CategoryId = categoryId,
                Varianter = request.Varianter,
                IsRequired = request.IsRequired,
                OptionId = request.OptionId,
                OptionValueIds = request.OptionValueIds
            });
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        // DELETE api/v1/[controller]/option-value/{categoryOptionValueId}
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryOptionValue))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpDelete("option-value/{categoryOptionValueId}")]
        public async Task<IActionResult> DeleteCategoryOptionValue(string categoryOptionValueId)
        {
            var result = await _mediator.Send(new DeleteCategoryOptionValueCommand
            {
                Id = categoryOptionValueId
            });
            return result.Success ? Ok() : BadRequest(result.Message);
        }

        // GET api/v1/[controller]/{categoryId}/option-values
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryOptionValueDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("{categoryId}/option-values")]
        public async Task<IActionResult> GetCategoryOptionValues(string categoryId)
        {
            var result = await _mediator.Send(new GetCategoryOptionValuesByIdQuery
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

        // GET api/v1/[controller]?isActive=null
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
            var result = await _mediator.Send(new GetCategoryByIdQuery
            {
                Id = id
            });
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }
    }
}