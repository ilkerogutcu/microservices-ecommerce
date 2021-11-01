using Catalog.Domain.Common;

namespace Catalog.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public double Rating { get; set; }
    }
}