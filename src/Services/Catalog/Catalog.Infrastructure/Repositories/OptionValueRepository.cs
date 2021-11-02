using System.Linq;
using System.Threading.Tasks;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class OptionValueRepository : MongoDbRepositoryBase<OptionValue>, IOptionValueRepository
    {
        public OptionValueRepository(ICatalogContext<OptionValue> context) : base(context)
        {
        }
// // Find the documents to delete
//         var test = db.GetCollection<Entity>("test");
//         var filter = new BsonDocument();
//         var docs = test.Find(filter).ToList();
//
// // Get the _id values of the found documents
//         var ids = docs.Select(d => d.id);
//
// // Create an $in filter for those ids
//         var idsFilter = Builders<Entity>.Filter.In(d => d.id, ids);
//
// // Delete the documents using the $in filter
//         var result = test.DeleteMany(idsFilter);

        public async Task<bool> DeleteManyByOptionId(string optionId)
        {
            var optionValues = await Collection.FindAsync(x => x.OptionId.Equals(optionId));

            var ids = optionValues.ToList().Select(x => x.Id);

            var result = await Collection.DeleteManyAsync(Builders<OptionValue>.Filter.In(x => x.Id, ids));
            return result.DeletedCount > 0;
        }
    }
}