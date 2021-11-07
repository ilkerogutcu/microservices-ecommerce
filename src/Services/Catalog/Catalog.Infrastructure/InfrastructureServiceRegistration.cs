using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Common;
using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Olcsan.Boilerplate.Utilities.IoC;

namespace Catalog.Infrastructure
{
    public class InfrastructureServiceRegistration : ICoreModule
    {
        public void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(typeof(ICatalogContext<>), typeof(CatalogContext<>));
            serviceCollection.AddSingleton(typeof(IDocumentDbRepository<>), typeof(MongoDbRepositoryBase<>));
           
            serviceCollection.AddSingleton<IBrandRepository, BrandRepository>();
            serviceCollection.AddSingleton<IProductRepository, ProductRepository>();
            serviceCollection.AddSingleton<IOptionRepository, OptionRepository>();
            serviceCollection.AddSingleton<IOptionValueRepository, OptionValueRepository>();
            serviceCollection.AddSingleton<ICategoryRepository, CategoryRepository>();
            serviceCollection.AddSingleton<ICategoryOptionValueRepository, CategoryOptionValueRepository>();

        }
    }
}