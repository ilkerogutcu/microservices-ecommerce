using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;
using Order.Application.Constants;
using Order.Application.Interfaces.Repositories;
using Order.Application.Interfaces.Services;
using Order.Domain.AggregateModels.BuyerAggregate;
using Order.Domain.Events;
using Serilog;

namespace Order.Application.DomainEventHandlers
{
    public class OrderStartedDomainEventHandler : INotificationHandler<OrderStartedDomainEvent>
    {
        private readonly IBuyerRepository _buyerRepository;


        public OrderStartedDomainEventHandler(IBuyerRepository buyerRepository)
        {
            _buyerRepository = buyerRepository;
        }

        public async Task Handle(OrderStartedDomainEvent notification, CancellationToken cancellationToken)
        {
            var cardTypeId = notification.CardTypeId != 0 ? notification.CardTypeId : 1;
            var buyer = await _buyerRepository.GetAsync(x => x.UserId == notification.UserId, x => x.PaymentMethods);

            var buyerExisted = buyer != null;
            if (!buyerExisted)
            {
                buyer = new Buyer(notification.Email, notification.UserId, notification.FirstName, notification.LastName);
            }

            buyer.VerifyOrAddPaymentMethod(cardTypeId, $"Payment Method on {DateTime.UtcNow}", notification.CardNumber,
                notification.CardSecurityNumber, notification.CardHolderName, notification.CardExpiration, notification.Order.Id);

            var result = buyerExisted ? _buyerRepository.Update(buyer) : await _buyerRepository.AddAsync(buyer);
            await _buyerRepository.SaveChangesAsync();
        }
    }
}