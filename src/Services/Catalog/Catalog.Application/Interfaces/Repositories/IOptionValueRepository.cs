using System.Threading.Tasks;
using Catalog.Domain.Common;
using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces.Repositories
{
    public interface IOptionValueRepository: IDocumentDbRepository<OptionValue>
    {
        Task<bool> DeleteManyByOptionId(string optionId);
    }
}