using System;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Identity.API.Extensions;
using Identity.Application.DependencyResolvers;
using Identity.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Identity.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var logFilePath = $"{projectDirectory + "/logs"}/{DateTime.Now:yyyy-MM-dd}.txt";
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(logFilePath,
                    retainedFileCountLimit: 1,
                    fileSizeLimitBytes: 5000000,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}")
                .CreateLogger();
            CreateHostBuilder(args).Build().MigrateDatabase<IdentityContext>((context, services) =>
                {
                    
                    // IdentityContextSeed
                    //     .SeedAsync(context, services)
                    //     .Wait();
                })
                .Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder => { builder.RegisterModule(new AutofacApplicationModule()); })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}