using Catalog.Domain.Common;

namespace Catalog.Domain.Entities
{
    public class Brand : BaseEntity
    {
        public string Name { get; set; }
        public string NormalizedName => Name?.ToLower();
        public bool IsActive { get; set; }
    }
}