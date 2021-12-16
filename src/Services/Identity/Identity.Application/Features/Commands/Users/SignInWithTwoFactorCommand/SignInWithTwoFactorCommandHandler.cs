using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Identity.Application.Constants;
using Identity.Application.Features.Commands.Users.ViewModels;
using Identity.Application.Features.Events.Users.UserSignedInEvent;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Commands.Users.SignInWithTwoFactorCommand
{
    public class SignInWithTwoFactorCommandHandler : IRequestHandler<SignInWithTwoFactorCommand, IDataResult<SignInResponse>>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IMediator _mediator;

        public SignInWithTwoFactorCommandHandler(SignInManager<User> signInManager, UserManager<User> userManager, IMediator mediator)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mediator = mediator;
        }

        [ExceptionLogAspect(typeof(FileLogger), "Identity-Service")]
        [LogAspect(typeof(FileLogger), "Identity-Service")]
        public async Task<IDataResult<SignInResponse>> Handle(SignInWithTwoFactorCommand request,
            CancellationToken cancellationToken)
        {
            var result = await _signInManager.TwoFactorSignInAsync("Email", request.Code, true, false);
            if (!result.Succeeded)
            {
                return new ErrorDataResult<SignInResponse>(Messages.SignInFailed);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            var userRoles = await _userManager.GetRolesAsync(user);
            _mediator.Publish(new UserSignedInEvent(request.IpAddress, user));
            return new SuccessDataResult<SignInResponse>(new SignInResponse
            {
                Id = user.Id,
                Email = user.Email,
                Roles = userRoles.ToList(),
            }, Messages.SignedInSuccessfully);
        }
    }
}