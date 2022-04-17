using System;
using System.Threading.Tasks;
using EventBus.Base.Abstraction;
using MediatR;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Order.API.IntegrationEvents.Events;
using Order.Application.Features.Commands.Orders.CreateOrderCommand;
using Serilog;

namespace Order.API.IntegrationEvents.EventHandlers
{
    public class OrderCreatedIntegrationEventHandler : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public OrderCreatedIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(OrderCreatedIntegrationEvent @event)
        {
            try
            {
                Log.Information(
                    $"Handling Integration Event: {@event.Id} at {typeof(Startup).Namespace} - {nameof(OrderCreatedIntegrationEventHandler)}");
                var createOrderCommand = new CreateOrderCommand(@event.Basket.Items, @event.UserId, @event.FirstName, @event.LastName,
                    @event.PhoneNumber, @event.Email, @event.City, @event.District, @event.Zip, @event.AddressLine, @event.AddressTitle,
                    @event.CardNumber, @event.CardHolderName, @event.CardExpirationMonth, @event.CardExpirationYear, @event.CardSecurityNumber,
                    @event.CardTypeId);

                await _mediator.Send(createOrderCommand);
                Log.Information("Integration Event Handled: {@event.Id}", @event.Id);
            }
            catch (Exception e)
            {
                Log.Error(e, "Error handling Integration Event: {@event.Id}", @event.Id);
            }
        }
    }
}