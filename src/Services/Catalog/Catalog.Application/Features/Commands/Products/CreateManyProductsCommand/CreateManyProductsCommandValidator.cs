using Catalog.Application.Dtos;
using FluentValidation;

namespace Catalog.Application.Features.Commands.Products.CreateManyProductsCommand
{
    public class CreateManyProductsCommandValidator : AbstractValidator<CreateManyProductsCommand>
    {
        public CreateManyProductsCommandValidator()
        {
            RuleForEach(x => x.Products).NotEmpty().SetValidator(new CreateProductValidator());
        }
    }

    public class CreateProductValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product name cannot be empty");
            RuleFor(x => x.BrandId).NotEmpty().WithMessage("Brand id cannot be empty");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category id cannot be empty");
            RuleFor(x => x.ImageUrls.Count).GreaterThan(0).WithMessage("Image required! Please upload images.");
            RuleFor(x => x.ImageUrls.Count).LessThanOrEqualTo(6).WithMessage("Maximum image count be 6");
            RuleFor(x => x.LongDescription).NotEmpty().WithMessage("Long description cannot be empty");
            RuleFor(x => x.ShortDescription).NotEmpty().WithMessage("Short description cannot be empty");
            RuleFor(x => x.ModelCode).NotEmpty().WithMessage("Model code cannot be empty");
            RuleFor(x => x.Barcode).NotEmpty().WithMessage("Barcode cannot be empty");
            RuleFor(x => x.StockCode).NotEmpty().WithMessage("StockCode code cannot be empty");
            RuleFor(x => x.SalePrice).GreaterThan(0).WithMessage("Sale price must be greater than 0");
            RuleFor(x => x.SalePrice).GreaterThanOrEqualTo(x => x.SalePrice).When(x => x.ListPrice.HasValue)
                .WithMessage("List price must be greater than or equal to sale price");
            RuleFor(x => x.StockQuantity).GreaterThanOrEqualTo(0).WithMessage("Stock quantity must be greater than  or equal to 0");

        }
    }
}