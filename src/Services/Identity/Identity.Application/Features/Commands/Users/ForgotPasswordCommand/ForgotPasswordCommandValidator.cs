using FluentValidation;

namespace Identity.Application.Features.Commands.Users.ForgotPasswordCommand
{
    public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email cannot be empty!");
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}