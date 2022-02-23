using System.Collections.Generic;
using Catalog.Grpc.Common;

namespace Catalog.Grpc.Entities
{
    public class CategoryOptionValue : BaseEntity
    {
        
        public Category Category { get; set; }
        public Option Option { get; set; }
        public bool IsRequired { get; set; }
        public bool Varianter { get; set; }
        public bool Slicer { get; set; }
        public List<OptionValue>  OptionValues { get; set; }= new List<OptionValue>();
    }
}