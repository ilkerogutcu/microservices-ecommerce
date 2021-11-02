using FluentValidation;

namespace Catalog.Application.Features.Commands.Options.DeleteOptionCommand
{
    public class DeleteOptionCommandValidator : AbstractValidator<DeleteOptionCommand>
    {
        public DeleteOptionCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id cannot be empty");
        }
    }
}