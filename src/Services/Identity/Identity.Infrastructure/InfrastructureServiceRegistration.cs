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


        }
    }
}