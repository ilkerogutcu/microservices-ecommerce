using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Identity.Application.Constants;
using Identity.Application.Features.Commands.Users.ViewModels;
using Identity.Application.Features.Events.Users.UserSignedInEvent;
using Identity.Application.Utilities;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Commands.Users.SignInWithTwoFactorCommand
{
    public class SignInWithTwoFactorCommandHandler : IRequestHandler<SignInWithTwoFactorCommand,
        IDataResult<SignInResponseViewModel>>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public SignInWithTwoFactorCommandHandler(SignInManager<User> signInManager, UserManager<User> userManager,
            IMediator mediator, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mediator = mediator;
            _configuration = configuration;
        }

        [ExceptionLogAspect(typeof(FileLogger), "Identity-Service")]
        [LogAspect(typeof(FileLogger), "Identity-Service")]
        public async Task<IDataResult<SignInResponseViewModel>> Handle(SignInWithTwoFactorCommand request,
            CancellationToken cancellationToken)
        {
            var result = await _signInManager.TwoFactorSignInAsync("Email", request.Code, true, false);
            if (!result.Succeeded)
            {
                return new ErrorDataResult<SignInResponseViewModel>(Messages.SignInFailed);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            var token = await AuthenticationHelper.GenerateJwtToken(user, _configuration, _userManager,
                request.IpAddress);
            var userRoles = await _userManager.GetRolesAsync(user);
            _mediator.Publish(new UserSignedInEvent(request.IpAddress, user));
            return new SuccessDataResult<SignInResponseViewModel>(new SignInResponseViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Roles = userRoles.ToList(),
                JwtToken = new JwtSecurityTokenHandler().WriteToken(token)
            }, Messages.SignedInSuccessfully);
        }
    }
}