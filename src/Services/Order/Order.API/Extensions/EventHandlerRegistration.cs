using Microsoft.Extensions.DependencyInjection;
using Order.API.IntegrationEvents.EventHandlers;

namespace Order.API.Extensions
{
    public static class EventHandlerRegistration
    {
        public static IServiceCollection ConfigureEventHandlers(this IServiceCollection services)
        {
            services.AddTransient<OrderCreatedIntegrationEventHandler>();
            return services;
        }
    }
}