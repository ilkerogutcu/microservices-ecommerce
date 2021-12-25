using System.Text.Json.Serialization;
using Identity.Application.Features.Commands.Users.ViewModels;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Commands.Users.SignInCommand
{
    public class SignInCommand : IRequest<IDataResult<SignInResponseViewModel>>
    {
        public SignInCommand(string email, string password, string ipAddress)
        {
            Email = email;
            Password = password;
            IpAddress = ipAddress;
        }

        public string Email { get; set; }
        public string Password { get; set;}
        [JsonIgnore]
        public string IpAddress { get; set; }
    }
}