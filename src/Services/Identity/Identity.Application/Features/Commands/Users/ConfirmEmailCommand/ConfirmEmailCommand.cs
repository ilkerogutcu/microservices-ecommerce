using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Commands.Users.ConfirmEmailCommand
{
    public class ConfirmEmailCommand : IRequest<IResult>
    {
        /// <summary>
        ///     User id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Generated token by identity service
        /// </summary>
        public string VerificationToken { get; set; }
    }
}