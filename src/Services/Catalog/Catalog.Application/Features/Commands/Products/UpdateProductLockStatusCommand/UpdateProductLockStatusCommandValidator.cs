using FluentValidation;

namespace Catalog.Application.Features.Commands.Products.UpdateProductLockStatusCommand
{
    public class UpdateProductLockStatusCommandValidator : AbstractValidator<UpdateProductLockStatusCommand>
    {
        public UpdateProductLockStatusCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id cannot be empty");
            RuleFor(x => x.LockStatus).NotNull().WithMessage("Lock status cannot be null");
        }
    }
}