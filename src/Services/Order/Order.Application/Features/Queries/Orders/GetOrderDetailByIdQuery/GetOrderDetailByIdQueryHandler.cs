using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;
using Order.Application.Constants;
using Order.Application.Features.Queries.ViewModels;
using Order.Application.Interfaces.Repositories;

namespace Order.Application.Features.Queries.Orders.GetOrderDetailByIdQuery
{
    public class GetOrderDetailByIdQueryHandler : IRequestHandler<GetOrderDetailByIdQuery, IDataResult<OrderDetailViewModel>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrderDetailByIdQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IDataResult<OrderDetailViewModel>> Handle(GetOrderDetailByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(x => x.Id == request.OrderId, x => x.OrderItems);
            if (order is null)
            {
                return new ErrorDataResult<OrderDetailViewModel>(Messages.DataNotFound);
            }

            var result = _mapper.Map<OrderDetailViewModel>(order);
            return new SuccessDataResult<OrderDetailViewModel>(result);
        }
    }
}