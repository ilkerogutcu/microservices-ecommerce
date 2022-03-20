using System;
using Basket.API.Core.Application.Mapping.CatalogMapping;
using Basket.API.Core.Application.Repository;
using Basket.API.Core.Application.Services;
using Basket.API.Extensions;
using Basket.API.Infrastructure.GrpcServices;
using Basket.API.Infrastructure.Repository;
using Basket.API.IntegrationEvents.EventHandlers;
using Basket.API.IntegrationEvents.Events;
using Catalog.Grpc.Protos;
using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Basket.API
{
    public class Startup
    {
        readonly string ApiCorsPolicy = "_apiCorsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAutoMapper(typeof(CatalogMappingProfile));
            services.AddHttpContextAccessor();
            services.AddCors(options =>
            {
                options.AddPolicy(ApiCorsPolicy, builder =>
                {
                    builder.WithOrigins("http://localhost:3000") 
                        .AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                });
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.HttpOnly = HttpOnlyPolicy.Always;
                options.Secure = CookieSecurePolicy.None;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });
            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation  
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Basket Service WEB API",
                    Description = "Basket Service WEB API"
                });
                // To Enable authorization using Swagger (JWT)  
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        System.Array.Empty<string>()
                    }
                });
            });
            services.ConfigureAuth(Configuration);
            services.AddSingleton(sp => sp.ConfigureRedis(Configuration));
            services.AddSingleton<IEventBus>(sp =>
            {
                EventBusConfig config = new()
                {
                    ConnectionRetryCount = 5,
                    EventNameSuffix = "IntegrationEvent",
                    SubscriberClientAppName = "BasketService",
                    EventBusType = EventBusType.RabbitMQ
                };
                return EventBusFactory.Create(config, sp);
            });
            services.AddSingleton<IBasketRepository, BasketRepository>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddSingleton<IBasketService, BasketService>();
            services.AddSingleton<ICatalogService, CatalogService>();

            services.AddTransient<OrderCreatedIntegrationEventHandler>();
            // Grpc Configuration
            services.AddGrpcClient<CatalogProtoService.CatalogProtoServiceClient>
                (o => o.Address = new Uri(Configuration["GrpcSettings:CatalogUrl"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1"));
            }

            app.UseRouting();
            app.UseCors(ApiCorsPolicy);

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            ConfigureSubscription(app.ApplicationServices);
        }

        private void ConfigureSubscription(IServiceProvider serviceProvider)
        {
            var eventBus = serviceProvider.GetRequiredService<IEventBus>();
            eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
        }
    }
}