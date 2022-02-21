using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Order.Application.Interfaces.Repositories;
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

        [ExceptionLogAspect(typeof(FileLogger), "OrderStartedDomainEventHandler")]
        public async Task Handle(OrderStartedDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var buyer = await _buyerRepository.GetAsync(x => x.UserId == notification.UserId);

                var buyerExisted = buyer != null;
                if (!buyerExisted)
                {
                    buyer = new Buyer(notification.Email, notification.UserId, notification.FirstName, notification.LastName);
                }

                buyer.VerifyBuyerMethod(notification.Order.Id);
                var result = buyerExisted ? _buyerRepository.Update(buyer) : await _buyerRepository.AddAsync(buyer);
                await _buyerRepository.SaveChangesAsync();
                Log.Information("Buyer {@buyer}", buyer);
            }
            catch (Exception e)
            {
                Log.Error(e, "OrderStartedDomainEventHandler");
            }
        }
    }
}