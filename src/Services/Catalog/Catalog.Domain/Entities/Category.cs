using System.Collections.Generic;
using Catalog.Domain.Common;
using MongoDB.Bson;

namespace Catalog.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string ParentId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<Category> SubCategories { get; set; } = new();

        public void AddSubCategory(Category category)
        {
            category.Id = ObjectId.GenerateNewId().ToString();
            category.ParentId = Id;
            SubCategories.Add(category);
        }

        public Category GetCategoryByIdFromSubCategories(List<Category> subCategories, string id)
        {
            var returnValue = new Category();
            foreach (var category in subCategories)
            {
                if (category.Id.Equals(id))
                {
                    returnValue = category;
                    break;
                }

                returnValue = GetCategoryByIdFromSubCategories(subCategories, id);
            }

            return returnValue;
        }
    }
}