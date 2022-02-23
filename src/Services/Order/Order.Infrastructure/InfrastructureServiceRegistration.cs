using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Olcsan.Boilerplate.Utilities.IoC;
using Order.Application.Interfaces.Repositories;
using Order.Application.Interfaces.Services;
using Order.Infrastructure.Context;
using Order.Infrastructure.GrpcServices;
using Order.Infrastructure.Repositories;

namespace Order.Infrastructure
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
            serviceCollection.AddDbContext<OrderDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                opt.EnableSensitiveDataLogging();
            });

            serviceCollection.AddSingleton<IBuyerRepository, BuyerRepository>();
            serviceCollection.AddSingleton<IOrderRepository, OrderRepository>();
            
            serviceCollection.AddSingleton<ICatalogService, CatalogService>();

            var optionsBuilder = new DbContextOptionsBuilder<OrderDbContext>().UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            using var dbContext = new OrderDbContext(optionsBuilder.Options, null);
            dbContext.Database.EnsureCreated();
            dbContext.Database.Migrate();
        }
    }
}