using System.Collections.Generic;
using Catalog.Application.Dtos;

namespace Catalog.Application.Features.Queries.Categories.GetCategoryOptionValuesByIdQuery
{
    public class GetCategoryOptionValuesByIdViewModel
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<CategoryOptionValueDto> CategoryOptionValues { get; set; }
    }
}