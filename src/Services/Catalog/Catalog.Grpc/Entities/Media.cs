using System;

namespace Catalog.Grpc.Entities
{
    public class Media
    {
        public string Url { get; set; }
        public string PublicId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}