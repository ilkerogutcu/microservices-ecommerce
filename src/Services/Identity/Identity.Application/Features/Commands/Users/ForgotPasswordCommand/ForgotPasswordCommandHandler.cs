using System;
using System.Threading;
using System.Threading.Tasks;
using Identity.Application.Constants;
using Identity.Application.Features.Events.Users.ForgotPasswordEvent;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.Aspects.Autofac.Validation;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Commands.Users.ForgotPasswordCommand
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, IResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMediator _mediator;

        public ForgotPasswordCommandHandler(UserManager<User> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        [ExceptionLogAspect(typeof(FileLogger), "Identity-Service")]
        [LogAspect(typeof(FileLogger), "Identity-Service")]
        [ValidationAspect(typeof(ForgotPasswordCommandValidator))]
        public async Task<IResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new SuccessResult(Messages.ForgotPassword);
            }
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            _mediator.Publish(new ForgotPasswordEvent(resetToken, user.Id, user.Email));

            return new SuccessResult(Messages.ForgotPassword);
        }
    }
}