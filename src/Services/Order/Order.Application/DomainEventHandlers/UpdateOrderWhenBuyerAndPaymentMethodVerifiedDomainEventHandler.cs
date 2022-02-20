using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Order.Application.Interfaces.Repositories;
using Order.Domain.Events;

namespace Order.Application.DomainEventHandlers
{
    public class UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler : INotificationHandler<BuyerAndPaymentMethodVerifiedDomainEvent>
    {
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task Handle(BuyerAndPaymentMethodVerifiedDomainEvent notification, CancellationToken cancellationToken)
        {
            var orderToUpdate = await _orderRepository.GetAsync(x => x.Id == notification.OrderId);
            orderToUpdate.SetBuyerId(notification.Buyer.Id);
            orderToUpdate.SetPaymentId(notification.PaymentMethod.Id);
        }
    }
}