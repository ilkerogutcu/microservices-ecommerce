using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Identity.Application.Features.Queries.Users.GetCurrentUserIdQuery;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Commands.Users.ChangePasswordCommand
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, IResult>
    {
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;

        public ChangePasswordCommandHandler(IMediator mediator, UserManager<User> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        public async Task<IResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = await _mediator.Send(new GetCurrentUserIdQuery(), cancellationToken);
            if (currentUserId == default)
            {
                return new ErrorResult("User not found");
            }

            var existingUser = await _userManager.FindByIdAsync(currentUserId.ToString());
            if (existingUser == null)
            {
                return new ErrorResult("User not found");
            }

            if (request.NewPassword != request.ConfirmNewPassword)
            {
                return new ErrorResult("Passwords do not match");
            }

            var changePasswordResult =
                await _userManager.ChangePasswordAsync(existingUser, request.OldPassword, request.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                return new ErrorResult(changePasswordResult.Errors.First().Description);
            }

            return new SuccessResult("Password changed successfully");
        }
    }
}