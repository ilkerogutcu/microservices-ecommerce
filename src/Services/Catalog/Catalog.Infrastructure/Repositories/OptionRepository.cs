using System.Collections.Generic;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Models;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class OptionRepository : MongoDbRepositoryBase<Option>, IOptionRepository
    {
        public OptionRepository(ICatalogContext<Option> context) : base(context)
        {
        }
    }
}