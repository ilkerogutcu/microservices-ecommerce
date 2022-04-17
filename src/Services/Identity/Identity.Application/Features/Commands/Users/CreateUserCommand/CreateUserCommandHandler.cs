using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Identity.Application.Constants;
using Identity.Application.Features.Queries.Users.ViewModels;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.Aspects.Autofac.Validation;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Commands.Users.CreateUserCommand
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, IResult>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateUserCommandHandler(RoleManager<IdentityRole> roleManager, UserManager<User> userManager,
            IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [ExceptionLogAspect(typeof(FileLogger), "Identity-Service")]
        [LogAspect(typeof(FileLogger), "Identity-Service")]
        [ValidationAspect(typeof(CreateUserCommandValidator))]
        public async Task<IResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value))
            {
                return new ErrorDataResult<UserViewModel>(Messages.SignInFirst);
            }

            var currentUser =
                await _userManager.FindByEmailAsync(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)
                    ?.Value);
            if (currentUser is null)
            {
                return new ErrorDataResult<UserViewModel>(Messages.SignInFirst);
            }

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser is not null)
            {
                return new ErrorResult(Messages.UserAlreadyExists);
            }

            var user = _mapper.Map<User>(request);

            user.UserName = user.Email;
            user.CreatedDate = DateTime.Now;
            user.IsActive = true;
            user.EmailConfirmed = true;
            user.CreatedBy = currentUser.Id;
            var createUserResult = await _userManager.CreateAsync(user, request.Password);
            if (!createUserResult.Succeeded)
            {
                return new ErrorResult(Messages.SignUpFailed +
                                       $":{createUserResult.Errors.ToList()[0].Description}");
            }

            foreach (var role in request.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(role)) continue;
                var result = await _userManager.AddToRoleAsync(user, role);
                if (result.Succeeded) continue;
                await _userManager.DeleteAsync(user);
                return new ErrorResult(Messages.CreateUserFailed);
            }

            return new SuccessResult(Messages.UserCreatedSuccessfully);
        }
    }
}