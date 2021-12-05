using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Application.Dtos;
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

        // public async Task<OptionValueDetailsDto> GetAllDetailsByOptionIdAsync(string optionId)
        // {
        //     var option = await (await new CatalogContext<Option>().Options.FindAsync(x => x.Id.Equals(optionId)))
        //         .FirstOrDefaultAsync();
        //     var optionValues = await (await Collection.FindAsync(x => x.OptionId.Equals(optionId))).ToListAsync();
        //     var result = new OptionValueDetailsDto
        //     {
        //         Option = new OptionDto
        //         {
        //             Id = option.Id,
        //             Name = option.Name,
        //             Varianter = option.Varianter,
        //             IsActive = option.IsActive,
        //             IsRequired = option.IsRequired
        //         },
        //         OptionValue = (from optionValue in optionValues
        //             where optionValue.OptionId.Equals(option.Id)
        //             select new OptionValueDto
        //             {
        //                 Id = optionValue.Id,
        //                 Name = optionValue.Name
        //             }).ToList()
        //     };
        //     return result;
        // }

        public async Task<List<OptionWithValuesDto>> GetAllDetailsAsync()
        {
            var options = await (await new CatalogContext<Option>().Options.FindAsync(x => true))
                .ToListAsync();
            var optionValues = await (await Collection.FindAsync(x => true)).ToListAsync();
            var result = (from option in options
                select new OptionWithValuesDto
                {
                    OptionId = option.Id,
                    OptionName = option.Name,
                    OptionValues = (from optionValue in optionValues
                        where optionValue.OptionId.Equals(option.Id)
                        select new OptionValueDto
                        {
                            Id = optionValue.Id,
                            Name = optionValue.Name
                        }).ToList()
                }).ToList();
            return result;
        }

        public async Task<bool> DeleteManyByOptionIdAsync(string optionId)
        {
            var optionValues = await Collection.FindAsync(x => x.OptionId.Equals(optionId));

            var ids = optionValues.ToList().Select(x => x.Id);

            var result = await Collection.DeleteManyAsync(Builders<OptionValue>.Filter.In(x => x.Id, ids));
            return result.DeletedCount > 0;
        }
    }
}