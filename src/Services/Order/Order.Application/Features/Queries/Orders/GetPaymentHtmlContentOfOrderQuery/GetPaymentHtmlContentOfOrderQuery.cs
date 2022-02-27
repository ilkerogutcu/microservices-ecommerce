using System.Threading.Tasks;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;
using Order.Application.Features.Queries.ViewModels;

namespace Order.Application.Features.Queries.Orders.GetPaymentHtmlContentOfOrderQuery
{
    public class GetPaymentHtmlContentOfOrderQuery : IRequest<IDataResult<PaymentContentOfOrderViewModel>>
    {
       
    }
}