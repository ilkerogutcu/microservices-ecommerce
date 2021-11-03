using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Domain.Common;
using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces.Repositories
{
    public interface IOptionValueRepository: IDocumentDbRepository<OptionValue>
    {
        Task<bool> DeleteManyByOptionIdAsync(string optionId);
       // Task<OptionValueDetailsDto> GetAllDetailsByOptionIdAsync(string optionId);
        Task<List<OptionValueDetailsDto>> GetAllDetailsAsync();

    }
}