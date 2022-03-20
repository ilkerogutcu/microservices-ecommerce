using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Identity.Application.Constants;
using Identity.Application.Features.Events.Users.SendEmailConfirmationTokenEvent;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
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
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public SignUpCommandHandler(UserManager<User> userManager,
            IMapper mapper, IMediator mediator)
        {
            _userManager = userManager;
            _mapper = mapper;
            _mediator = mediator;
        }

       
        [ValidationAspect(typeof(SignUpCommandValidator))]
        public async Task<IDataResult<SignUpResponse>> Handle(SignUpCommand request,
            CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser is not null)
            {
                return new ErrorDataResult<SignUpResponse>(Messages.UserAlreadyExists);
            }

            var user = _mapper.Map<User>(request);
            switch (request.Gender)
            {
                case 0:
                    user.Gender = Gender.Male;
                    break;
                case 1:
                    user.Gender = Gender.Female;
                    break;
                case 2:
                    user.Gender = Gender.Other;
                    break;
                default:
                    return new ErrorDataResult<SignUpResponse>(Messages.SignUpFailed);
            }

            user.UserName = user.Email;
            user.CreatedDate = DateTime.Now;
            user.IsActive = true;
            user.TwoFactorEnabled = true;

            var createUserResult = await _userManager.CreateAsync(user, request.Password);
            if (!createUserResult.Succeeded)
            {
                return new ErrorDataResult<SignUpResponse>(Messages.CreateUserFailed +
                                                           $":{createUserResult.Errors.ToList()[0].Description}");
            }


            var result = await _userManager.AddToRoleAsync(user, nameof(Role.Buyer));
            if (!result.Succeeded)
            {
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