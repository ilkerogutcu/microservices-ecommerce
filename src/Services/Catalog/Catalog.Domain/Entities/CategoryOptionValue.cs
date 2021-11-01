using Catalog.Domain.Common;

namespace Catalog.Domain.Entities
{
    public class CategoryOptionValue : BaseEntity
    {
        public OptionValue OptionValue { get; set; }
        public Category Category { get; set; }
    }
}