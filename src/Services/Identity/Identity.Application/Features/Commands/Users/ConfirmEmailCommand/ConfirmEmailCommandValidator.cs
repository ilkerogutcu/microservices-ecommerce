using FluentValidation;

namespace Identity.Application.Features.Commands.Users.ConfirmEmailCommand
{
    public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().NotNull().WithMessage("First name cannot be empty or null!");
            RuleFor(x => x.VerificationToken).NotEmpty().NotNull()
                .WithMessage("Verification token cannot be empty or null!");
        }
    }
}