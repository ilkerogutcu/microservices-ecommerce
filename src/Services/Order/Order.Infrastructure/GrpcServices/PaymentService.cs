using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Order.Application.Interfaces.Services;
using Payment.Grpc.Protos;
using Serilog;

namespace Order.Infrastructure.GrpcServices
{
    public class PaymentService : IPaymentService
    {
        private readonly PaymentProtoService.PaymentProtoServiceClient _paymentProtoService;

        public PaymentService(PaymentProtoService.PaymentProtoServiceClient paymentProtoService)
        {
            _paymentProtoService = paymentProtoService;
        }

        public async Task<CreatePaymentResponse> CreateThreeDPaymentAsync(PaymentRequest request)
        {
            try
            {
                var response = await _paymentProtoService.CreateThreeDPaymentAsync(request);
                Log.Information($"CreateThreeDPaymentAsync: {JsonConvert.SerializeObject(response)}");
                return response;
            }
            catch (Exception e)
            {
                Log.Error(e, "Error while creating 3D payment");
                return null;
            }
        }

        public async Task<CheckThreeDPaymentResponse> CheckThreeDPaymentAsync(CheckThreeDPaymentRequest request)
        {
            try
            {
                var response = await _paymentProtoService.CheckThreeDPaymentAsync(request);
                Log.Information($"CheckThreeDPaymentAsync: {JsonConvert.SerializeObject(response)}");
                return response;
            }
            catch (Exception e)
            {
                Log.Error(e, "Error while checking 3D payment");
                return null;
            }
        }
    }
}