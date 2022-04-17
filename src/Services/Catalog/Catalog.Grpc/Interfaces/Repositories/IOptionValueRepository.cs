using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Grpc.Entities;

namespace Catalog.Grpc.Interfaces.Repositories
{
    public interface IOptionValueRepository: IDocumentDbRepository<OptionValue>
    {
    }
}