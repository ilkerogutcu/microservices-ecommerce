﻿using System.Text.Json.Serialization;
using Identity.Application.Features.Commands.Users.ViewModels;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Commands.Users.SignInWithTwoFactorCommand
{
    public class SignInWithTwoFactorCommand : IRequest<IDataResult<SignInResponseViewModel>>
    {
        public string Code { get; set; }
        public string Email { get; set; }
        [JsonIgnore] public string IpAddress { get; set; }
    }
}