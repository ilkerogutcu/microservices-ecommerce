using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Serilog;

namespace Order.API.Extensions
{
    public static class HostExtension
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetService<TContext>();

                try
                {
                    Log.Information("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                    var retry = Policy.Handle<SqlException>()
                        .WaitAndRetry(new TimeSpan[]
                        {
                            TimeSpan.FromSeconds(3),
                            TimeSpan.FromSeconds(5),
                            TimeSpan.FromSeconds(10),
                            TimeSpan.FromSeconds(15),
                        });
                    retry.Execute(() => InvokeSeeder(seeder, context, services));
                    Log.Information("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
                }
                catch (Exception e)
                {
                    Log.Error(e, "An error occurred while migrating the database.");
                }
            }

            return host;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services)
            where TContext : DbContext
        {
            context.Database.EnsureCreated();
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}