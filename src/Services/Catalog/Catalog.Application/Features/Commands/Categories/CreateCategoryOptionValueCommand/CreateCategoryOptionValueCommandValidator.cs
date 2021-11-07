using FluentValidation;

namespace Catalog.Application.Features.Commands.Categories.CreateCategoryOptionValueCommand
{
    public class CreateCategoryOptionValueCommandValidator: AbstractValidator<CreateCategoryOptionValueCommand>
    {
        public CreateCategoryOptionValueCommandValidator()
        {
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category id cannot be empty!");
            RuleFor(x => x.OptionValueIds).NotNull().WithMessage("Option value id cannot be empty!");
        }
    }
}