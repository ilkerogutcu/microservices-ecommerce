using Catalog.Domain.Common;

namespace Catalog.Domain.Entities
{
    public class Brand : BaseEntity
    {
        public Brand(string name, bool isActive)
        {
            Name = name;
            IsActive = isActive;
            NormalizedName = name.ToLower();
        }

        public string Name { get; }

        public string NormalizedName { get; }

        public bool IsActive { get; }
    }
}