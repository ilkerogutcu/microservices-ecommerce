using System.Threading.Tasks;
using Catalog.Grpc.Entities;

namespace Catalog.Grpc.Interfaces.Repositories
{
    public interface ICategoryOptionValueRepository: IDocumentDbRepository<CategoryOptionValue>
    {
    }
}