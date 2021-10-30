using Catalog.Domain.Common;

namespace Catalog.Domain.Entities
{
    public class Media : BaseEntity
    {
        public string Url { get; set; }
        public string PublicId { get; set; }
    }
}