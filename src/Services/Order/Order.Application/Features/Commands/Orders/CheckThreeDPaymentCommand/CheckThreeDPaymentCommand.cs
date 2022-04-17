using MediatR;
using Olcsan.Boilerplate.Utilities.Results;
using Order.Application.Features.Queries.ViewModels;

namespace Order.Application.Features.Commands.Orders.CheckThreeDPaymentCommand
{
    public class CheckThreeDPaymentCommand : IRequest<IDataResult<CheckPaymentViewModel>>
    {
        public string Status { get; set; }
        public string PaymentId { get; set; }
        public string ConversationData { get; set; }
        public string ConversationId { get; set; }
        public string MdStatus { get; set; }

        public CheckThreeDPaymentCommand(string status, string paymentId, string conversationData, string conversationId, string mdStatus)
        {
            Status = status;
            PaymentId = paymentId;
            ConversationData = conversationData;
            ConversationId = conversationId;
            MdStatus = mdStatus;
        }
    }
}