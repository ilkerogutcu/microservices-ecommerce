using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Identity.Application.Constants;
using Identity.Application.Features.Events.Users.SendEmailConfirmationTokenEvent;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.Aspects.Autofac.Validation;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Commands.Users.SignUpCommand
{
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, IDataResult<SignUpResponse>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public SignUpCommandHandler(RoleManager<IdentityRole> roleManager, UserManager<User> userManager,
            IMapper mapper, IMediator mediator)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
            _mediator = mediator;
        }
        
        [ExceptionLogAspect(typeof(FileLogger), "Identity-Service")]
        [LogAspect(typeof(FileLogger), "Identity-Service")]
        [ValidationAspect(typeof(SignUpCommandValidator))]
        public async Task<IDataResult<SignUpResponse>> Handle(SignUpCommand request,
            CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser is not null)
            {
                return new ErrorDataResult<SignUpResponse>(Messages.UserAlreadyExists);
            }

            foreach (var role in request.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    return new ErrorDataResult<SignUpResponse>(Messages.RoleNotFound);
                }
            }

            var user = _mapper.Map<User>(request);
            user.UserName = user.Email;
            user.CreatedDate = DateTime.Now;
            user.IsActive = true;
            var createUserResult = await _userManager.CreateAsync(user, request.Password);
            if (!createUserResult.Succeeded)
            {
                return new ErrorDataResult<SignUpResponse>(Messages.SignUpFailed +
                                                           $":{createUserResult.Errors.ToList()[0].Description}");
            }

            foreach (var role in request.Roles)
            {
                var result = await _userManager.AddToRoleAsync(user, role);
                if (result.Succeeded) continue;
                await _userManager.DeleteAsync(user);
                return new ErrorDataResult<SignUpResponse>(Messages.SignUpFailed);
            }
            
            _mediator.Publish(new SendEmailConfirmationTokenEvent(user.Id));
            return new SuccessDataResult<SignUpResponse>(new SignUpResponse()
            {
                Email = user.Email,
                Id = user.Id
            }, Messages.SentConfirmationEmailSuccessfully);
        }
    }
}