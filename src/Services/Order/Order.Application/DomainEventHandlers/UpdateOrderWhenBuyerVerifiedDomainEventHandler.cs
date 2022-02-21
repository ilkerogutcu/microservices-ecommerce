using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Order.Application.Interfaces.Repositories;
using Order.Domain.Events;
using Serilog;

namespace Order.Application.DomainEventHandlers
{
    public class UpdateOrderWhenBuyerVerifiedDomainEventHandler : INotificationHandler<BuyerVerifiedDomainEvent>
    {
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderWhenBuyerVerifiedDomainEventHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task Handle(BuyerVerifiedDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var orderToUpdate = await _orderRepository.GetByIdAsync(notification.OrderId);
                orderToUpdate.SetBuyerId(notification.Buyer.Id);
                Log.Information("Order {@Order} updated", orderToUpdate);
            }
            catch (Exception e)
            {
                Log.Error(e, "Error updating order");
            }
        }
    }
}