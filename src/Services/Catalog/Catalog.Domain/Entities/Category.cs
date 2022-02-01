using System;
using System.Collections.Generic;
using Catalog.Domain.Common;
using MongoDB.Bson;

namespace Catalog.Domain.Entities
{
    public  class Category : BaseEntity
    {
        public string ParentId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public IList<Category> SubCategories { get; set; } = new List<Category>();

        public void AddSubCategory(Category category)
        {
            category.Id = ObjectId.GenerateNewId().ToString();
            category.ParentId = Id;
            SubCategories.Add(category);
        }
        
        public Category Update(string name, bool isActive, string lastUpdatedBy)
        {
            Name = name;
            IsActive = isActive;
            LastUpdatedDate = DateTime.Now;
            LastUpdatedBy = lastUpdatedBy;
            return this;
        }
    }
}