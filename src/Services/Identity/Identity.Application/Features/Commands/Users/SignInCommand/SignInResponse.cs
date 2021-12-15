﻿using System.Collections.Generic;

namespace Identity.Application.Features.Commands.Users.SignInCommand
{
    public class SignInResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public bool TwoFAIsEnabled { get; set; }
        public bool IsVerified { get; set; }
        public string JwtToken { get; set; }
    }
}