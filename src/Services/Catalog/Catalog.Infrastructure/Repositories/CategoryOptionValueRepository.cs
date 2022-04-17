using System.Linq;
using System.Threading.Tasks;
using Catalog.Application.Features.Queries.Categories.GetCategoryOptionValuesByIdQuery;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class CategoryOptionValueRepository : MongoDbRepositoryBase<CategoryOptionValue>,
        ICategoryOptionValueRepository
    {
        public CategoryOptionValueRepository(ICatalogContext<CategoryOptionValue> context) : base(context)
        {
        }
  
        public async Task<GetCategoryOptionValuesByIdViewModel> GetCategoryOptionValuesByIdAsync(string id)
        {
            var categoryOptionValues = Collection.Find(x => x.Category.Id.Equals(id)).ToList();
            if (categoryOptionValues.Count<=0)
            {
                return null;
            }

            return null;

        }
    }
}