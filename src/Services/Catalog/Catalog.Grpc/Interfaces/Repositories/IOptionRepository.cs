using Catalog.Grpc.Entities;

namespace Catalog.Grpc.Interfaces.Repositories
{
    public interface IOptionRepository : IDocumentDbRepository<Option>
    {
    }
}