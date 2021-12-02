using System.Threading.Tasks;
using Catalog.Application.Features.Commands.Medias.UploadMediaCommand;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MediaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MediaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST api/v1/[controller]/
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Domain.Entities.Media))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] UploadMediaCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }
    }
}