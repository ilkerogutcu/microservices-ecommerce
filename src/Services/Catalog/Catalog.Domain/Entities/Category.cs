using System.Collections.Generic;
using Catalog.Domain.Common;

namespace Catalog.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string ParentId { get; set; }

        public string Name { get; set; }
        public bool IsActive { get; set; }
        public IList<Category> Nodes { get; set; } = new List<Category>();
        
        public void AddNode(Category category)
        {
            category.ParentId = this.ParentId;
            Nodes.Add(category);
        }
    }
}