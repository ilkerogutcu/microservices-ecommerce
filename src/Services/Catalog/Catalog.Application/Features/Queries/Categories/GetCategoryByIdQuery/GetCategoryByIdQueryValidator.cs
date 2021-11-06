using FluentValidation;

namespace Catalog.Application.Features.Queries.Categories.GetCategoryByIdQuery
{
    public class GetCategoryByIdQueryValidator : AbstractValidator<GetCategoryByIdQuery>
    {
        public GetCategoryByIdQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Category id cannot be empty!");
        }
    }
}