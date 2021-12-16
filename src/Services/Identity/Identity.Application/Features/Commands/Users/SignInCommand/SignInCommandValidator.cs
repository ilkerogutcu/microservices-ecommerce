using FluentValidation;

namespace Identity.Application.Features.Commands.Users.SignInCommand
{
    public class SignInCommandValidator : AbstractValidator<SignInCommand>
    {
        public SignInCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email cannot be empty!");
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password cannot be empty!");
        }
    }
}