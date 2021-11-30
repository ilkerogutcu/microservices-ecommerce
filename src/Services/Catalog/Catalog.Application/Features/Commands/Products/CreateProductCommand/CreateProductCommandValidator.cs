using System.Data;
using FluentValidation;

namespace Catalog.Application.Features.Commands.Products.CreateProductCommand
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product name cannot be empty");
            RuleFor(x => x.BrandId).NotEmpty().WithMessage("Brand id cannot be empty");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category id cannot be empty");
            RuleFor(x => x.FileList.Count).GreaterThan(0).WithMessage("Image required! Please upload images.");
            RuleFor(x => x.FileList.Count).LessThan(6).WithMessage("Maximum image count be 6");
            RuleFor(x => x.Skus.Count).GreaterThan(1).WithMessage("You must add a sku");
            RuleFor(x => x.LongDescription).NotEmpty().WithMessage("Long description cannot be empty");
            RuleFor(x => x.ShortDescription).NotEmpty().WithMessage("Short description cannot be empty");
            RuleFor(x => x.ModelCode).NotEmpty().WithMessage("Model code cannot be empty");
        }
    }
}