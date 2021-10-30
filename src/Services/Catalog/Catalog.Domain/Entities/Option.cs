using Catalog.Domain.Common;

namespace Catalog.Domain.Entities
{
    public class Options:BaseEntity
    {
        public string Name { get; set; }
        public bool IsRequired { get; set; }
    }
}