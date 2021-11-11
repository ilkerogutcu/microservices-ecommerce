using System.Collections.Generic;
using Catalog.Application.Dtos;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.Categories.GetAllCategoriesQuery
{
    public class GetAllCategoriesQuery : IRequest<IDataResult<List<CategoryDto>>>
    {
        public bool? IsActive { get; set; }
    }
}