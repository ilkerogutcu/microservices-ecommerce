using FluentValidation;

namespace Catalog.Application.Features.Commands.Categories.DeleteCategoryCommand
{
    public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryCommandValidator()
        {
            RuleFor(x => x.MainCategoryId).NotEmpty().WithMessage("Main category id cannot be empty!");
        }
    }
}