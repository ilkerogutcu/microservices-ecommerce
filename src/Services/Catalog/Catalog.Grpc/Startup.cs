using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Catalog.Grpc.Interfaces.Repositories;
using Catalog.Grpc.Mapping;
using Catalog.Grpc.Persistence;
using Catalog.Grpc.Repositories;
using Catalog.Grpc.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Olcsan.Boilerplate.DependencyResolvers;
using Olcsan.Boilerplate.Extensions;
using Olcsan.Boilerplate.Utilities.IoC;

namespace Catalog.Grpc
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddAutoMapper(typeof(CatalogMappingProfile));

            services.AddDependencyResolvers(new ICoreModule[]
            {
                new CoreModule(),
            });
            services.AddSingleton(typeof(ICatalogContext<>), typeof(CatalogContext<>));
            services.AddSingleton(typeof(IDocumentDbRepository<>), typeof(MongoDbRepositoryBase<>));

            services.AddSingleton<IBrandRepository, BrandRepository>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IOptionRepository, OptionRepository>();
            services.AddSingleton<IOptionValueRepository, OptionValueRepository>();
            services.AddSingleton<ICategoryRepository, CategoryRepository>();
            services.AddSingleton<ICategoryOptionValueRepository, CategoryOptionValueRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ServiceTool.ServiceProvider = app.ApplicationServices;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<CatalogService>();

                endpoints.MapGet("/",
                    async context =>
                    {
                        await context.Response.WriteAsync(
                            "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                    });
            });
        }
    }
}