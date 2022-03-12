using System;
using Identity.Application.Interfaces;
using Identity.Application.Interfaces.Repositories;
using Identity.Infrastructure.GrpcServices;
using Identity.Infrastructure.Repositories;
using Mail.Grpc.Protos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Olcsan.Boilerplate.Utilities.IoC;

namespace Identity.Infrastructure
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
            serviceCollection.AddSingleton<IMailService, MailService>();
            serviceCollection.AddSingleton<IDistrictRepository, DistrictRepository>();
            serviceCollection.AddSingleton<IAddressRepository, AddressRepository>();
            serviceCollection.AddSingleton<ICityRepository, CityRepository>();

            // Grpc Configuration
            serviceCollection.AddGrpcClient<MailProtoService.MailProtoServiceClient>
                (o => o.Address = new Uri(Configuration["GrpcSettings:MailUrl"]));
        }
    }
}