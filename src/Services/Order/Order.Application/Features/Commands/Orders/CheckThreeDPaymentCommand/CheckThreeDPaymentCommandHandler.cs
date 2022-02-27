using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using Olcsan.Boilerplate.Utilities.Results;
using Order.Application.Constants;
using Order.Application.Features.Queries.ViewModels;
using Order.Application.Interfaces.Repositories;
using Order.Application.Interfaces.Services;
using Order.Domain.AggregateModels.OrderAggregate;
using Payment.Grpc.Protos;
using Serilog;

namespace Order.Application.Features.Commands.Orders.CheckThreeDPaymentCommand
{
    public class CheckThreeDPaymentCommandHandler : IRequestHandler<CheckThreeDPaymentCommand, IDataResult<CheckPaymentViewModel>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentService _paymentService;

        public CheckThreeDPaymentCommandHandler(IOrderRepository orderRepository, IPaymentService paymentService)
        {
            _orderRepository = orderRepository;
            _paymentService = paymentService;
        }

        public async Task<IDataResult<CheckPaymentViewModel>> Handle(CheckThreeDPaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {

                if (request.Status.Equals("failure"))
                {
                    return new ErrorDataResult<CheckPaymentViewModel>(Messages.PaymentFailed);
                }

                if (string.IsNullOrEmpty(request.ConversationId))
                {
                    return new ErrorDataResult<CheckPaymentViewModel>(Messages.DataNotFound);
                }

                if (string.IsNullOrEmpty(request.PaymentId))
                {
                    return new ErrorDataResult<CheckPaymentViewModel>(Messages.DataNotFound);
                }

                if (string.IsNullOrEmpty(request.MdStatus))
                {
                    return new ErrorDataResult<CheckPaymentViewModel>(Messages.DataNotFound);
                }

                var order = await _orderRepository.GetAsync(x =>
                    x.Id.ToString().Equals(request.ConversationId) && x.OrderStatus.Equals(OrderStatus.WaitingForPayment));
                if (order is null)
                {
                    return new ErrorDataResult<CheckPaymentViewModel>(Messages.DataNotFound);
                }

                var checkPaymentRequest = new CheckThreeDPaymentRequest();
                if (!string.IsNullOrEmpty(request.ConversationData))
                {
                    checkPaymentRequest.ConservationData = request.ConversationData;
                }

                checkPaymentRequest.ConservationId = request.ConversationId;
                checkPaymentRequest.PaymentId = request.PaymentId;
                var checkPaymentResponse = await _paymentService.CheckThreeDPaymentAsync(checkPaymentRequest);
                Log.Information($"{JsonConvert.SerializeObject(checkPaymentResponse)}");
                if (checkPaymentResponse.Status.Equals("success"))
                {
                    order.orderStatusId = OrderStatus.Paid.Id;
                    _orderRepository.Update(order);
                    return new SuccessDataResult<CheckPaymentViewModel>(Messages.SuccessfullyPayment);
                }

                return new ErrorDataResult<CheckPaymentViewModel>(Messages.PaymentFailed);
            }
            catch (Exception e)
            {
                Log.Error(e, "Error in CheckThreeDPaymentCommandHandler");
                return new ErrorDataResult<CheckPaymentViewModel>(Messages.PaymentFailed);
            }
        }
    }
}