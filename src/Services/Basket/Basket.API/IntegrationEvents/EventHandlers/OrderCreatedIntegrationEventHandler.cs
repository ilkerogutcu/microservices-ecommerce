using System.Threading.Tasks;
using Basket.API.Core.Application.Services;
using Basket.API.IntegrationEvents.Events;
using EventBus.Base.Abstraction;
using Serilog;

namespace Basket.API.IntegrationEvents.EventHandlers
{
    public class OrderCreatedIntegrationEventHandler : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
    {
        private readonly IBasketService _basketService;

        public OrderCreatedIntegrationEventHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public Task Handle(OrderCreatedIntegrationEvent @event)
        {
            Log.Information(
                "----- Handling integration event: {IntegrationEventId} at - ({@IntegrationEvent})",
                @event.Id, @event);
             _basketService.DeleteBasketAsync(@event.UserId);
            return Task.FromResult(1);
        }
    }
}