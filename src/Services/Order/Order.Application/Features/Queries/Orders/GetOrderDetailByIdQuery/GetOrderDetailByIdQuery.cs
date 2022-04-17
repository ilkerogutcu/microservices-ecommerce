using System;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;
using Order.Application.Features.Queries.ViewModels;

namespace Order.Application.Features.Queries.Orders.GetOrderDetailByIdQuery
{
    public class GetOrderDetailByIdQuery : IRequest<IDataResult<OrderDetailViewModel>>
    {
        public Guid OrderId { get; set; }

        public GetOrderDetailByIdQuery(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}