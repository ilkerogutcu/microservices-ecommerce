using FluentValidation;

namespace Catalog.Application.Features.Commands.CreateOptionCommand
{
    public class CreateOptionCommandValidator : AbstractValidator<CreateOptionCommand>
    {
        public CreateOptionCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.Varianter).NotNull().WithMessage("Varianter cannot be empty");
            RuleFor(x => x.IsActive).NotNull().WithMessage("IsActive cannot be empty");
            RuleFor(x => x.IsRequired).NotNull().WithMessage("IsRequired cannot be empty");
        }
    }
}