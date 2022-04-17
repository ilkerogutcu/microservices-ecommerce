using FluentValidation;

namespace Catalog.Application.Features.Commands.Categories.UpdateCategoryCommand
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.MainCategoryId).NotEmpty().WithMessage("Main category id cannot be empty!");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Category name cannot be empty!");
            RuleFor(x => x.IsActive).NotNull().WithMessage("Category isActive field not cannot be null!");
        }
    }
}