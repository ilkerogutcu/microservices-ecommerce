using FluentValidation;

namespace Catalog.Application.Features.Commands.Categories.CreateOrUpdateCategoryOptionValueCommand
{
    public class CreateOrUpdateCategoryOptionValueCommandValidator: AbstractValidator<CreateOrUpdateCategoryOptionValueCommand>
    {
        public CreateOrUpdateCategoryOptionValueCommandValidator()
        {
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category id cannot be empty!");
            RuleFor(x => x.OptionValueIds).NotNull().WithMessage("Option value id cannot be empty!");
        }
    }
}