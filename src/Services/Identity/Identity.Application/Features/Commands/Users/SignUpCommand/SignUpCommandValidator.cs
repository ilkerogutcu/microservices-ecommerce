using FluentValidation;

namespace Identity.Application.Features.Commands.Users.SignUpCommand
{
    public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
    {
        public SignUpCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name cannot be empty!");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name cannot be empty!");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email cannot be empty!");
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Roles.Count).GreaterThanOrEqualTo(1).WithMessage("The user must have a role.");
            RuleFor(x => x.Password).Equal(x=>x.ConfirmPassword).WithMessage("Passwords do not match");
            RuleFor(x => x.BirthDate).NotNull().WithMessage("Birth date cannot be null!");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password cannot be empty!");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Confirm Password cannot be empty!");
        }
    }
}