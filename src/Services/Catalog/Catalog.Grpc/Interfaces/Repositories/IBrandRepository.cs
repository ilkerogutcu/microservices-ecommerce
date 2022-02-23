using Catalog.Grpc.Entities;

namespace Catalog.Grpc.Interfaces.Repositories
{
    public interface IBrandRepository : IDocumentDbRepository<Brand>
    {
    }
}