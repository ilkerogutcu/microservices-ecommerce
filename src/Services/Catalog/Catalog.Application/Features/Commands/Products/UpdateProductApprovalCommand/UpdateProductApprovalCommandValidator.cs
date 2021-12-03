using FluentValidation;

namespace Catalog.Application.Features.Commands.Products.UpdateProductApprovalCommand
{
    public class UpdateProductApprovalCommandValidator : AbstractValidator<UpdateProductApprovalCommand>
    {
        public UpdateProductApprovalCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id cannot be empty");
            RuleFor(x => x.Approve).NotNull().WithMessage("Is active cannot be null");
        }
    }
}