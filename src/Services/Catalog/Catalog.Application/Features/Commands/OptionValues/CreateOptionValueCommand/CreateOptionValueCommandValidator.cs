using FluentValidation;

namespace Catalog.Application.Features.Commands.OptionValues.CreateOptionValueCommand
{
    public class CreateOptionValueCommandValidator : AbstractValidator<CreateOptionValueCommand>
    {
        public CreateOptionValueCommandValidator()
        {
            RuleFor(x => x.OptionId).NotEmpty().WithMessage("OptionId cannot be empty");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
        }
    }
}