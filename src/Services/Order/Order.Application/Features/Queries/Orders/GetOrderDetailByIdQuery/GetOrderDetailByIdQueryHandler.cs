using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;
using Order.Application.Constants;
using Order.Application.Features.Queries.ViewModels;
using Order.Application.Interfaces.Repositories;
using Serilog;

namespace Order.Application.Features.Queries.Orders.GetOrderDetailByIdQuery
{
    public class GetOrderDetailByIdQueryHandler : IRequestHandler<GetOrderDetailByIdQuery, IDataResult<OrderDetailViewModel>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrderDetailByIdQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }


        public async Task<IDataResult<OrderDetailViewModel>> Handle(GetOrderDetailByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var order = await _orderRepository.GetAsync(x => x.Id == request.OrderId, x => x.OrderItems, 
                    x => x.Address, x => x.OrderStatus);
                if (order is null)
                {
                    return new ErrorDataResult<OrderDetailViewModel>(Messages.DataNotFound);
                }

                var result = _mapper.Map<OrderDetailViewModel>(order);
                return new SuccessDataResult<OrderDetailViewModel>(result);
            }
            catch (Exception e)
            {
                Log.Error(e, "Error in GetOrderDetailByIdQueryHandler");
                return new ErrorDataResult<OrderDetailViewModel>();
            }
        }
    }
}