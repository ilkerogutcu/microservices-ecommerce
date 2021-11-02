using FluentValidation;

namespace Catalog.Application.Features.Commands.Options.UpdateOptionCommand
{
    public class UpdateOptionCommandValidator: AbstractValidator<UpdateOptionCommand>
    {
        public UpdateOptionCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id cannot be empty");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.Varianter).NotNull().WithMessage("Varianter cannot be null");
            RuleFor(x => x.IsActive).NotNull().WithMessage("IsActive cannot be null");
            RuleFor(x => x.IsRequired).NotNull().WithMessage("IsRequired cannot be null");
        }
    }
}