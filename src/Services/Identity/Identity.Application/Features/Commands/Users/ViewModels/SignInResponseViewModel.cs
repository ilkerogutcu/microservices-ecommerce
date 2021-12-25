using System.Collections.Generic;

namespace Identity.Application.Features.Commands.Users.ViewModels
{
    public class SignInResponseViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string JwtToken { get; set; }
    }
}