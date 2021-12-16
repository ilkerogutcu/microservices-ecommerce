﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Identity.Application.Constants;
using Identity.Application.Features.Commands.Users.ViewModels;
using Identity.Application.Features.Events.Users.SendEmailTwoFactorTokenEvent;
using Identity.Application.Features.Events.Users.UserSignedInEvent;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Commands.Users.SignInCommand
{
    public class SignInCommandHandler : IRequestHandler<SignInCommand, IDataResult<SignInResponse>>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMediator _mediator;

        public SignInCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager,
            IMediator mediator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mediator = mediator;
        }

        [ExceptionLogAspect(typeof(FileLogger), "Identity-Service")]
        [LogAspect(typeof(FileLogger), "Identity-Service")]
        public async Task<IDataResult<SignInResponse>> Handle(SignInCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new ErrorDataResult<SignInResponse>(Messages.EmailOrPasswordIsIncorrect);
            }

            if (!user.EmailConfirmed)
            {
                return new ErrorDataResult<SignInResponse>(Messages.EmailIsNotConfirmed);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password,
                true, true);
            if (result.IsLockedOut)
            {
                return new ErrorDataResult<SignInResponse>(
                    $"Your account is locked out. Please wait for {user.LockoutEnd} try again");
            }

            if (result.RequiresTwoFactor)
            {
                _mediator.Publish(new SendEmailTwoFactorTokenEvent(user.Id));
                return new ErrorDataResult<SignInResponse>(Messages.Sent2FaCodeEmailSuccessfully);
            }

            if (!result.Succeeded)
            {
                return new ErrorDataResult<SignInResponse>(Messages.EmailOrPasswordIsIncorrect);
            }

            
            var userRoles = await _userManager.GetRolesAsync(user);
            _mediator.Publish(new UserSignedInEvent(request.IpAddress, user));
            return new SuccessDataResult<SignInResponse>(new SignInResponse
            {
                Id = user.Id,
                Email = user.Email,
                Roles = userRoles.ToList()
            }, Messages.SignedInSuccessfully);
        }
    }
}