using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Payment.Grpc.Protos;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Olcsan.Boilerplate.Utilities.IoC;
using BasketItem = Iyzipay.Model.BasketItem;

namespace Payment.Grpc.Services
{
    public class PaymentService : PaymentProtoService.PaymentProtoServiceBase
    {
        private readonly Options _options;
        private readonly string _locale;

        public PaymentService()

        {
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();

            _options = new Options
            {
                ApiKey = configuration["IyzicoPaymentConfig:ApiKey"],
                SecretKey = configuration["IyzicoPaymentConfig:SecretKey"],
                BaseUrl = configuration["IyzicoPaymentConfig:BaseUrl"],
            };
            _locale = configuration["IyzicoPaymentConfig:Locale"];
        }

        public override Task<CreatePaymentResponse> CreateThreeDPayment(PaymentRequest request, ServerCallContext context)
        {
            var createPaymentRequest = GetCreatePaymentRequest(request);


            var basketItems = GetBasketItems(request).ToList();

            createPaymentRequest.PaymentCard = GetPaymentCard(request);
            createPaymentRequest.Buyer = GetBuyer(request);
            createPaymentRequest.ShippingAddress = GetAddress(request);
            createPaymentRequest.BillingAddress = GetAddress(request);
            createPaymentRequest.BasketItems = basketItems;

            var result = ThreedsInitialize.Create(createPaymentRequest, _options);

            return Task.FromResult(new CreatePaymentResponse
            {
                Status = result.Status,
                ConservationId = result.ConversationId,
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                HtmlContent = result.HtmlContent
            });
        }

        public override Task<CheckThreeDPaymentResponse> CheckThreeDPayment(CheckThreeDPaymentRequest request, ServerCallContext context)
        {
            var createThreedsPaymentRequest = new CreateThreedsPaymentRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = request.ConservationId,
                PaymentId = request.PaymentId,
                ConversationData = request.ConservationData
            };
            var threedsPayment = ThreedsPayment.Create(createThreedsPaymentRequest, _options);

            return Task.FromResult(new CheckThreeDPaymentResponse()
            {
                Status = threedsPayment.Status,
                ErrorCode = threedsPayment.ErrorCode,
                ErrorMessage = threedsPayment.ErrorMessage,
                PaymentId = threedsPayment.PaymentId,
                ConservationId = threedsPayment.ConversationId,
                PaymentStatus = threedsPayment.PaymentStatus,
            });
        }

        private PaymentCard GetPaymentCard(PaymentRequest request)
        {
            return new PaymentCard()
            {
                CardHolderName = request.CardHolderName,
                CardNumber = request.CardNumber,
                ExpireMonth = request.ExpireMonth,
                ExpireYear = request.ExpireYear,
                Cvc = request.Cvc,
                RegisterCard = 0
            };
        }

        private CreatePaymentRequest GetCreatePaymentRequest(PaymentRequest request)
        {
            return new CreatePaymentRequest()
            {
                Locale = _locale,
                ConversationId = request.ConservationId,
                Price = request.Price,
                PaidPrice = request.PaidPrice,
                Currency = Currency.TRY.ToString(),
                Installment = 1,
                PaymentChannel = PaymentChannel.WEB.ToString(),
                PaymentGroup = PaymentGroup.PRODUCT.ToString(),
                CallbackUrl = request.CallbackUrl
            };
        }

        private Buyer GetBuyer(PaymentRequest request)
        {
            return new Buyer
            {
                Id = request.BuyerId,
                Name = request.BuyerName,
                Surname = request.BuyerSurname,
                GsmNumber = request.BuyerGsmNumber,
                Email = request.BuyerEmailAddress,
                IdentityNumber = request.BuyerIdentityNumber,
                LastLoginDate = request.BuyerLastLoginDate,
                RegistrationDate = request.BuyerRegistrationDate,
                RegistrationAddress = request.BuyerRegistrationAddress,
                Ip = request.BuyerIp,
                City = request.BuyerCity,
                Country = request.BuyerCountry,
                ZipCode = request.BuyerZipCode
            };
        }

        private Address GetAddress(PaymentRequest request)
        {
            return new Address
            {
                ContactName = request.BuyerName + " " + request.BuyerSurname,
                City = request.BuyerCity,
                Country = request.BuyerCountry,
                Description = request.ShippingAddress,
                ZipCode = request.BuyerZipCode
            };
        }

        private IEnumerable<BasketItem> GetBasketItems(PaymentRequest request)
        {
            return request.BasketItems.BasketItem.Select(basketItem => new BasketItem
            {
                Id = basketItem.ProductId,
                Name = basketItem.ProductName,
                Category1 = basketItem.ProductCategory,
                ItemType = BasketItemType.PHYSICAL.ToString(),
                Price = basketItem.ProductPrice,
            });
        }
    }
}