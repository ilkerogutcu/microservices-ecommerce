using FluentValidation;

namespace Catalog.Application.Features.Commands.Categories.CreateCategoryCommand
{
    public class CreateCategoryCommandValidator: AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Brand name cannot be empty!");
            RuleFor(x => x.IsActive).NotNull().WithMessage("Brand isActive field not cannot be null!");
        }
    }
}