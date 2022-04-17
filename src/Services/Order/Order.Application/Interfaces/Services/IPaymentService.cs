using System.Threading.Tasks;
using Payment.Grpc.Protos;

namespace Order.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<CreatePaymentResponse> CreateThreeDPaymentAsync(PaymentRequest request);
        Task<CheckThreeDPaymentResponse> CheckThreeDPaymentAsync(CheckThreeDPaymentRequest request);
    }
}