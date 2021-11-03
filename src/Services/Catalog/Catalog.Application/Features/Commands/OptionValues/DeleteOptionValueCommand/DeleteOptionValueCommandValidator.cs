using FluentValidation;

namespace Catalog.Application.Features.Commands.OptionValues.DeleteOptionValueCommand
{
    public class DeleteOptionValueCommandValidator: AbstractValidator<DeleteOptionValueCommand>
    {
        public DeleteOptionValueCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id cannot be empty");
        }
    }
}