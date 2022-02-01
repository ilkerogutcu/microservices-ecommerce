using FluentValidation;

namespace Catalog.Application.Features.Commands.Products.AddCommentToProductCommand
{
    public class AddCommentToProductCommandValidator : AbstractValidator<AddCommentToProductCommand>
    {
        public AddCommentToProductCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product Id cannot be empty");
            RuleFor(x => x.ProductRating).GreaterThanOrEqualTo(1).WithMessage("Rating greater than or equal to 1");
            RuleFor(x => x.ProductRating).LessThanOrEqualTo(5).WithMessage("Rating less than or equal to 5");
        }
    }
}