using System.Collections.Generic;

namespace Catalog.Application.Dtos
{
    public class CategoryDto
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public IList<CategoryDto> SubCategories { get; set; }
    }
}