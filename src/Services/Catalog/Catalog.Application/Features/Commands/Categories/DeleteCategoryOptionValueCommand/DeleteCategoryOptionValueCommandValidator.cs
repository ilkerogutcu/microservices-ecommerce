using FluentValidation;

namespace Catalog.Application.Features.Commands.Categories.DeleteCategoryOptionValueCommand
{
    public class DeleteCategoryOptionValueCommandValidator : AbstractValidator<DeleteCategoryOptionValueCommand>
    {
        public DeleteCategoryOptionValueCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Category id cannot be empty!");
        }
    }
}