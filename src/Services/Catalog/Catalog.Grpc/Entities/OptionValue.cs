using Catalog.Grpc.Common;

namespace Catalog.Grpc.Entities
{
    public class OptionValue : BaseEntity
    {
        public string OptionId { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
    }
}