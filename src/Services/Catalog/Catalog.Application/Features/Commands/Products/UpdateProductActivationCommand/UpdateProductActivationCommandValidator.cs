using FluentValidation;

namespace Catalog.Application.Features.Commands.Products.UpdateProductActivationCommand
{
    public class UpdateProductActivationCommandValidator : AbstractValidator<UpdateProductActivationCommand>
    {
        public UpdateProductActivationCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id cannot be empty");
            RuleFor(x => x.IsActive).NotNull().WithMessage("Is active cannot be null");
        }
    }
}