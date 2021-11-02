using FluentValidation;

namespace Catalog.Application.Features.Commands.OptionValues.UpdateOptionValueCommand
{
    public class UpdateOptionValueCommandValidator : AbstractValidator<UpdateOptionValueCommand>
    {
        public UpdateOptionValueCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id cannot be empty");
            RuleFor(x => x.OptionId).NotEmpty().WithMessage("OptionId cannot be empty");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
        }
    }
}