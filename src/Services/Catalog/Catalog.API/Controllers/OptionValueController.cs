using System.Threading.Tasks;
using Catalog.Application.Features.Commands.OptionValues.CreateOptionValueCommand;
using Catalog.Application.Features.Commands.OptionValues.UpdateOptionValueCommand;
using Catalog.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OptionValueController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OptionValueController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        // POST api/v1/[controller]/
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OptionValue))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOptionValueCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }
        
        // PUT api/v1/[controller]/
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OptionValue))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateOptionValueCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }
    }
}