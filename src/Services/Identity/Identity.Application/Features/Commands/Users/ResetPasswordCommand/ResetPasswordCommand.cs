using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Commands.Users.ResetPasswordCommand
{
    public class ResetPasswordCommand : IRequest<IResult>
    {
        /// <summary>
        ///     User id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Generated token by identity service
        /// </summary>
        public string ResetPasswordToken { get; set; }

        /// <summary>
        ///     Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     This variable validates for first password entered
        /// </summary>
        public string ConfirmPassword { get; set; }
    }
}