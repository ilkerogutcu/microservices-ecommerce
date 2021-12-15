using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Identity.Application.Constants;
using Identity.Application.Features.Commands.Users.ViewModels;
using Identity.Application.Utilities;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.IoC;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Commands.Users.SignInWithTwoFactorCommand
{
    public class SignInWithTwoFactorCommandHandler : IRequestHandler<SignInWithTwoFactorCommand, IDataResult<SignInResponse>>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;


        public SignInWithTwoFactorCommandHandler(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
        }

        [ExceptionLogAspect(typeof(FileLogger), "Identity-Service")]
        [LogAspect(typeof(FileLogger), "Identity-Service")]
        public async Task<IDataResult<SignInResponse>> Handle(SignInWithTwoFactorCommand request,
            CancellationToken cancellationToken)
        {
            var result = await _signInManager.TwoFactorSignInAsync("Email", request.Code, false, false);
            if (!result.Succeeded)
            {
                return new ErrorDataResult<SignInResponse>(Messages.SignInFailed);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            var token = await AuthenticationHelper.GenerateJwtToken(user, _configuration, _userManager,
                request.IpAddress);
            var userRoles = await _userManager.GetRolesAsync(user);
            return new SuccessDataResult<SignInResponse>(new SignInResponse
            {
                Id = user.Id,
                Email = user.Email,
                Roles = userRoles.ToList(),
                TwoFAIsEnabled = user.TwoFactorEnabled,
                IsVerified = user.EmailConfirmed,
                JwtToken = new JwtSecurityTokenHandler().WriteToken(token)
            }, Messages.SignedInSuccessfully);
        }
    }
}