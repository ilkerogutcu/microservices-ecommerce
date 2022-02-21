using System.Collections.Generic;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;
using Order.Application.Features.Queries.ViewModels;

namespace Order.Application.Features.Queries.Orders.GerOrderDetailsOfCurrentUserQuery
{
    public class GetOrderDetailsOfCurrentUserQuery : IRequest<IDataResult<List<OrderDetailViewModel>>>
    {
    }
}