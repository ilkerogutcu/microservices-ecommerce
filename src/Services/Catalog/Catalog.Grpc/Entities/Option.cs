using Catalog.Grpc.Common;

namespace Catalog.Grpc.Entities
{
    public class Option : BaseEntity
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public bool IsActive { get; set; }
    }
}