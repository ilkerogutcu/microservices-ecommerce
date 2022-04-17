using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;
using Order.Application.Constants;
using Order.Application.Features.Queries.ViewModels;
using Order.Application.Interfaces.Repositories;
using Order.Application.Interfaces.Services;
using Order.Domain.AggregateModels.OrderAggregate;
using Serilog;

namespace Order.Application.Features.Queries.Orders.GetPaymentHtmlContentOfOrderQuery
{
    public class GetPaymentHtmlContentOfOrderQueryHandler : IRequestHandler<GetPaymentHtmlContentOfOrderQuery,
        IDataResult<PaymentContentOfOrderViewModel>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IIdentityService _identityService;

        public GetPaymentHtmlContentOfOrderQueryHandler(IOrderRepository orderRepository, IIdentityService identityService)
        {
            _orderRepository = orderRepository;
            _identityService = identityService;
        }

        public async Task<IDataResult<PaymentContentOfOrderViewModel>> Handle(GetPaymentHtmlContentOfOrderQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = await _identityService.GetUserIdAsync();
                if (currentUserId == default)
                {
                    return new ErrorDataResult<PaymentContentOfOrderViewModel>(Messages.SignInFirst);
                }

                var lastWaitingForPaymentOrderOfCurrentUser = await _orderRepository.GetAsync(
                    x => x.Buyer.UserId == currentUserId && x.OrderStatus.Equals(OrderStatus.WaitingForPayment),
                    x => x.Buyer);
                if(lastWaitingForPaymentOrderOfCurrentUser == null)
                {
                    return new ErrorDataResult<PaymentContentOfOrderViewModel>("Sipariş bulunamadı");
                }
                return new SuccessDataResult<PaymentContentOfOrderViewModel>(new PaymentContentOfOrderViewModel()
                {
                    BuyerId = lastWaitingForPaymentOrderOfCurrentUser.Buyer.Id.ToString(),
                    HtmlContent = lastWaitingForPaymentOrderOfCurrentUser.PaymentHtmlContent,
                    OrderId = lastWaitingForPaymentOrderOfCurrentUser.Id.ToString()
                });
            }
            catch (Exception e)
            {
                Log.Error(e, "Error while getting payment html content of order");
                return new ErrorDataResult<PaymentContentOfOrderViewModel>("Error occured while getting payment html content of order");
            }
        }
    }
}