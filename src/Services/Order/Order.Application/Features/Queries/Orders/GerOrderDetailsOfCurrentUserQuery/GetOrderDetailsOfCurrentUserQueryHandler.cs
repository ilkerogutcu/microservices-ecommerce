using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;
using Order.Application.Constants;
using Order.Application.Features.Queries.ViewModels;
using Order.Application.Interfaces.Repositories;
using Order.Application.Interfaces.Services;
using Serilog;

namespace Order.Application.Features.Queries.Orders.GerOrderDetailsOfCurrentUserQuery
{
    public class GetOrderDetailsOfCurrentUserQueryHandler : IRequestHandler<GetOrderDetailsOfCurrentUserQuery,
        IDataResult<List<OrderDetailViewModel>>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetOrderDetailsOfCurrentUserQueryHandler(IOrderRepository orderRepository, IIdentityService identityService, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _identityService = identityService;
            _mapper = mapper;
        }

        public async Task<IDataResult<List<OrderDetailViewModel>>> Handle(GetOrderDetailsOfCurrentUserQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = await _identityService.GetUserIdAsync();
                if (currentUserId == default)
                {
                    return new ErrorDataResult<List<OrderDetailViewModel>>(Messages.SignInFirst);
                }

                var orders = await _orderRepository.GetListAsync(x => x.Buyer.UserId == currentUserId, x => x.OrderItems,
                    x => x.Address, x => x.OrderStatus);

                var result = _mapper.Map<List<OrderDetailViewModel>>(orders);

                return new SuccessDataResult<List<OrderDetailViewModel>>(result);
            }
            catch (Exception e)
            {
                Log.Error(e, "Error in GetOrderDetailsOfCurrentUserQueryHandler");
                return new ErrorDataResult<List<OrderDetailViewModel>>("Something went wrong");
            }
        }
    }
}