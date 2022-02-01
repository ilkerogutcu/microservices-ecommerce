using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Commands.Users.ForgotPasswordCommand
{
    public class ForgotPasswordCommand : IRequest<IResult>
    {
        public string Email { get; set; }
    }
}