using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Identity.Application.Constants;
using Identity.Application.Features.Commands.Users.AddAddressCommand;
using Identity.Application.Features.Commands.Users.ConfirmEmailCommand;
using Identity.Application.Features.Commands.Users.CreateUserCommand;
using Identity.Application.Features.Commands.Users.DeleteAddressCommand;
using Identity.Application.Features.Commands.Users.ForgotPasswordCommand;
using Identity.Application.Features.Commands.Users.ResetPasswordCommand;
using Identity.Application.Features.Commands.Users.SignInCommand;
using Identity.Application.Features.Commands.Users.SignInWithTwoFactorCommand;
using Identity.Application.Features.Commands.Users.SignUpCommand;
using Identity.Application.Features.Commands.Users.UpdateAddressCommand;
using Identity.Application.Features.Commands.Users.ViewModels;
using Identity.Application.Features.Events.Users.SendEmailConfirmationTokenEvent;
using Identity.Application.Features.Queries.Users.GetCurrentUserQuery;
using Identity.Application.Features.Queries.Users.ViewModels;
using Identity.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
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

        // POST api/v1/[controller]/me
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessDataResult<UserViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var result = await _mediator.Send(new GetCurrentUserQuery());
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        // POST api/v1/[controller]/sign-up
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessDataResult<SignUpResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        // POST api/v1/[controller]/create-user
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [Authorize(Roles = nameof(Role.Administrator))]
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        // POST api/v1/[controller]/send-email-verification-token/{userId}
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost("send-email-verification-token/{userId}")]
        public async Task<IActionResult> SendEmailConfirmationToken([FromRoute] string userId)
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest("User id cannot be empty!");

            await _mediator.Publish(new SendEmailConfirmationTokenEvent(userId));
            return Ok();
        }

        // POST api/v1/[controller]/sign-in
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessDataResult<SignInResponseViewModel>))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(SignInCommand command)
        {
            command.IpAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress?.ToString();
            var result = await _mediator.Send(command);
            if (result.Message.Equals(Messages.Sent2FaCodeEmailSuccessfully))
            {
                return Ok(result.Message);
            }

            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        // POST api/v1/[controller]/sign-in/2FA
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessDataResult<SignInResponseViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost("sign-in/2FA")]
        public async Task<IActionResult> SignInWithTwoFactorSecurity(SignInWithTwoFactorCommand command)
        {
            var ipAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress?.ToString();
            command.IpAddress = ipAddress;
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        // POST api/v1/[controller]/forgot-password
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        // POST api/v1/[controller]/reset-password
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        // POST api/v1/[controller]/me/address
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost("me/addresses")]
        public async Task<IActionResult> AddAddress(AddAddressToUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok() : BadRequest(result.Message);
        }

        // POST api/v1/[controller]/me/address/{addressId}
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut("me/addresses/{addressId:guid}")]
        public async Task<IActionResult> UpdateAddress([FromRoute] Guid addressId,
            [FromBody] UpdateAddressFromUserCommand command)
        {
            command.AddressId = addressId;
            var result = await _mediator.Send(command);
            return result.Success ? Ok() : BadRequest(result.Message);
        }

        // POST api/v1/[controller]/me/address/{addressId}
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpDelete("me/addresses/{addressId:guid}")]
        public async Task<IActionResult> DeleteAddress(Guid addressId)
        {
            var result = await _mediator.Send(new DeleteAddressFromUserCommand(addressId));
            return result.Success ? Ok() : BadRequest(result.Message);
        }

        // POST api/v1/[controller]?userId={userId}&confirmationToken={confirmationToken}
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok() : BadRequest(result.Message);
        }
    }
}