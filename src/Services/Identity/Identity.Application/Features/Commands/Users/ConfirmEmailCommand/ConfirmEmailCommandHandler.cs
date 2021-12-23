using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Identity.Application.Constants;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.Aspects.Autofac.Validation;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Commands.Users.ConfirmEmailCommand
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, IResult>
    {
        private readonly UserManager<User> _userManager;

        public ConfirmEmailCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [LogAspect(typeof(FileLogger), "Identity-Service")]
        [ExceptionLogAspect(typeof(FileLogger), "Identity-Service")]
        [ValidationAspect(typeof(ConfirmEmailCommandValidator))]
        public async Task<IResult> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }

            request.VerificationToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.VerificationToken));
            var result = await _userManager.ConfirmEmailAsync(user, request.VerificationToken);
            return result.Succeeded
                ? new SuccessResult()
                : new ErrorResult(Messages.ErrorVerifyingMail);
        }
    }
}