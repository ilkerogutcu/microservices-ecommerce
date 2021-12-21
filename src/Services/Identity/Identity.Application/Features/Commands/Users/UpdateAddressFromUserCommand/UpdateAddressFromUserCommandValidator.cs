using FluentValidation;

namespace Identity.Application.Features.Commands.Users.UpdateAddressFromUserCommand
{
    public class UpdateAddressFromUserCommandValidator : AbstractValidator<UpdateAddressFromUserCommand>
    {
        public UpdateAddressFromUserCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().NotNull().WithMessage("First name cannot be empty!");
            RuleFor(x => x.LastName).NotEmpty().NotNull().WithMessage("Last name cannot be empty!");
            RuleFor(x => x.DistrictId).NotEmpty().NotNull().WithMessage("District id cannot be empty!");
            RuleFor(x => x.PhoneNumber).NotEmpty().NotNull().WithMessage("Phone number cannot be empty!");
            RuleFor(x => x.AddressLine).NotEmpty().NotNull().WithMessage("Address line cannot be empty!");
            RuleFor(x => x.AddressTitle).NotEmpty().NotNull().WithMessage("Address line cannot be empty or null!");
            RuleFor(x => x.Zip).NotEmpty().NotNull().WithMessage("Zip cannot be empty or null!");
        }
    }
}