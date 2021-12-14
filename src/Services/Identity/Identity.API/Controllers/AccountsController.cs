using System.Threading.Tasks;
using Identity.Application.Features.Commands.Users.SignUpCommand;
using Identity.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/v1/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        // POST api/v1/[controller]/user/sign-up
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessDataResult<SignUpResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost("user/sign-up")]
        public async Task<IActionResult> UserSignUp([FromBody] SignUpCommand command)
        {
            command.Roles.Add(Roles.Buyer.ToString());
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
    }
}