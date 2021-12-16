using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Identity.Application.Constants;
using Identity.Application.Features.Events.Users.UserUpdatedEvent;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.Aspects.Autofac.Validation;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Commands.Users.ResetPasswordCommand
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, IResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMediator _mediator;

        public ResetPasswordCommandHandler(UserManager<User> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        [ExceptionLogAspect(typeof(FileLogger), "Identity-Service")]
        [LogAspect(typeof(FileLogger), "Identity-Service")]
        [ValidationAspect(typeof(ResetPasswordValidator))]
        public async Task<IResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }

            request.ResetPasswordToken =
                Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.ResetPasswordToken));
            var result = await _userManager.ResetPasswordAsync(user, request.ResetPasswordToken, request.Password);
            if (!result.Succeeded)
            {
                return new ErrorResult(Messages.PasswordResetFailed);
            }

            _mediator.Publish(new UserUpdatedEvent(user));
            return new SuccessResult(Messages.PasswordHasBeenResetSuccessfully);
        }
    }
}