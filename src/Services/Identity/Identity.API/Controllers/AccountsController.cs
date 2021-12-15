using System.Threading.Tasks;
using Identity.Application.Features.Commands.Users.SignInCommand;
using Identity.Application.Features.Commands.Users.SignUpCommand;
using Identity.Application.Features.Events.Users.SendEmailConfirmationTokenEvent;
using Identity.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Olcsan.Boilerplate.Utilities.Results;
using Microsoft.AspNetCore.HttpOverrides;
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

        // POST api/v1/[controller]/sign-up
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessDataResult<SignUpResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost("sign-up")]
        public async Task<IActionResult> UserSignUp([FromBody] SignUpCommand command)
        {
            command.Roles.Add(Roles.Buyer.ToString());
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost("send-email-verification-token/{userId}")]
        public async Task<IActionResult> SendEmailConfirmationToken([FromRoute] string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User id cannot be empty!");
            }

            await _mediator.Publish(new SendEmailConfirmationTokenEvent(userId));
            return Ok();
        }
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IDataResult<SignInResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(SignInCommand command)
        {
            command.IpAddress = (HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress)?.ToString();

            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
    }
}