using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Commands.Users.ChangePasswordCommand
{
    public class ChangePasswordCommand : IRequest<IResult>
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}