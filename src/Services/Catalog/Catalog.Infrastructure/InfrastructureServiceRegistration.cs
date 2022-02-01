using System;
using Catalog.Application.Interfaces;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Common;
using Catalog.Infrastructure.GrpcServices;
using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Repositories;
using Media.Grpc.Protos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Olcsan.Boilerplate.Utilities.IoC;

namespace Catalog.Infrastructure
{
    public class InfrastructureServiceRegistration : ICoreModule
    {
        public InfrastructureServiceRegistration(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
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
            serviceCollection.AddSingleton<IMediaGrpcService, MediaGrpcService>();
            // Grpc Configuration
            serviceCollection.AddGrpcClient<MediaProtoService.MediaProtoServiceClient>
                (o => o.Address = new Uri(Configuration["GrpcSettings:MediaUrl"]));
        }
    }
}