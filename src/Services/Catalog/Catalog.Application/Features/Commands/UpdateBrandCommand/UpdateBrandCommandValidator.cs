using FluentValidation;

namespace Catalog.Application.Features.Commands.UpdateBrandCommand
{
    public class UpdateBrandCommandValidator : AbstractValidator<UpdateBrandCommand>
    {
        public UpdateBrandCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id cannot be empty!");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Brand name cannot be empty!");
            RuleFor(x => x.IsActive).NotNull().WithMessage("Brand isActive field not cannot be null!");
        }
    }
}