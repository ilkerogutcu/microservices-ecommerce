using System.Collections.Generic;
using Catalog.Domain.Common;

namespace Catalog.Domain.Entities
{
    public class CategoryOptionValue : BaseEntity
    {
        public Category Category { get; set; }
        public List<OptionValue>  OptionValues { get; set; }= new List<OptionValue>();
    }
}