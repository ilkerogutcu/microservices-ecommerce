using FluentValidation;
using Identity.Application.Constants;

namespace Identity.Application.Features.Commands.Users.ResetPasswordCommand
{
    /// <summary>
    ///     Validator for reset password
    /// </summary>
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Password).Equal(x => x.ConfirmPassword)
                .WithMessage("Passwords don't match!");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User id  cannot be empty!");
            RuleFor(x => x.ResetPasswordToken).NotEmpty().WithMessage("Token cannot be empty!");
        }
    }
}