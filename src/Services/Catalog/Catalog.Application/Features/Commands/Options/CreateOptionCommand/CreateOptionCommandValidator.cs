﻿using FluentValidation;

namespace Catalog.Application.Features.Commands.Options.CreateOptionCommand
{
    public class CreateOptionCommandValidator : AbstractValidator<CreateOptionCommand>
    {
        public CreateOptionCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.IsActive).NotNull().WithMessage("IsActive cannot be null");
        }
    }
}