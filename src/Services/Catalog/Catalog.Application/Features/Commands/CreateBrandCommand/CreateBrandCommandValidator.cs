using FluentValidation;

namespace Catalog.Application.Features.Commands.CreateBrandCommand
{
    public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
    {
        public CreateBrandCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Brand name cannot be empty!");
            RuleFor(x => x.IsActive).NotNull().WithMessage("Brand isActive field not cannot be null!");
        }
    }
}