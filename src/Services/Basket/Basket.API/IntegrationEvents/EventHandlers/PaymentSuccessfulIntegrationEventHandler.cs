using System;
using System.Threading.Tasks;
using Basket.API.Core.Application.Services;
using Basket.API.IntegrationEvents.Events;
using EventBus.Base.Abstraction;
using Serilog;

namespace Basket.API.IntegrationEvents.EventHandlers
{
    public class PaymentSuccessfulIntegrationEventHandler : IIntegrationEventHandler<PaymentSuccessfulIntegrationEvent>
    {
        private readonly IBasketService _basketService;

        public PaymentSuccessfulIntegrationEventHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public Task Handle(PaymentSuccessfulIntegrationEvent @event)
        {
            try
            {
                Log.Information(
                    "----- Handling integration event: {IntegrationEventId} at - ({@IntegrationEvent})",
                    @event.Id, @event);
                _basketService.DeleteBasketAsync(@event.UserId);
                return Task.FromResult(1);
            }
            catch (Exception e)
            {
                Log.Error(e, "----- An error occurred while handling integration event: {IntegrationEventId}", @event.Id);
                return Task.FromResult(0);
            }
        }
    }
}