using FluentValidation;

namespace Catalog.Application.Features.Queries.Categories.GetCategoryOptionValuesByIdQuery
{
    public class GetCategoryOptionValuesByIdQueryValidator : AbstractValidator<GetCategoryOptionValuesByIdQuery>
    {
        public GetCategoryOptionValuesByIdQueryValidator()
        {
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category id cannot be empty!");
        }
    }
}