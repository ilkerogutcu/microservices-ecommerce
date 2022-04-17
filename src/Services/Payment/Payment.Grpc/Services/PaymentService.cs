using System;
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
using Serilog;
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
            try
            {
                var createPaymentRequest = GetCreatePaymentRequest(request);


                var basketItems = GetBasketItems(request).ToList();

                createPaymentRequest.PaymentCard = GetPaymentCard(request);
                createPaymentRequest.Buyer = GetBuyer(request);
                createPaymentRequest.ShippingAddress = GetAddress(request);
                createPaymentRequest.BillingAddress = GetAddress(request);
                createPaymentRequest.BasketItems = basketItems;

                var result = ThreedsInitialize.Create(createPaymentRequest, _options);
                var response = new CreatePaymentResponse();
                if (!string.IsNullOrEmpty(result.Status))
                {
                    response.Status = result.Status;
                }

                if (!string.IsNullOrEmpty(result.ConversationId))
                {
                    response.ConservationId = result.ConversationId;
                }

                if (!string.IsNullOrEmpty(result.ErrorCode))
                {
                    response.ErrorCode = result.ErrorCode;
                }

                if (!string.IsNullOrEmpty(result.ErrorMessage))
                {
                    response.ErrorCode = result.ErrorMessage;
                }

                if (!string.IsNullOrEmpty(result.HtmlContent))
                {
                    response.HtmlContent = result.HtmlContent;
                }

                return Task.FromResult(response);
            }
            catch (Exception e)
            {
                Log.Error(e, "Error in CreateThreeDPayment");
                return null;
            }
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
            var checkThreeDPaymentResponse = new CheckThreeDPaymentResponse();
            if (!string.IsNullOrEmpty(threedsPayment.Status))
            {
                checkThreeDPaymentResponse.Status = threedsPayment.Status;
            }

            if (!string.IsNullOrEmpty(threedsPayment.ErrorCode))
            {
                checkThreeDPaymentResponse.ErrorCode = threedsPayment.ErrorCode;
            }


            if (!string.IsNullOrEmpty(threedsPayment.ErrorMessage))
            {
                checkThreeDPaymentResponse.ErrorCode = threedsPayment.ErrorMessage;
            }

            if (!string.IsNullOrEmpty(threedsPayment.ConversationId))
            {
                checkThreeDPaymentResponse.ConservationId = threedsPayment.ConversationId;
            }

            if (!string.IsNullOrEmpty(threedsPayment.PaymentId))
            {
                checkThreeDPaymentResponse.PaymentId = threedsPayment.PaymentId;
            }

            if (!string.IsNullOrEmpty(threedsPayment.PaymentStatus))
            {
                checkThreeDPaymentResponse.PaymentStatus = threedsPayment.PaymentStatus;
            }

            return Task.FromResult(checkThreeDPaymentResponse);
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
                Price = request.Price.Replace(",", ""),
                PaidPrice = request.PaidPrice.Replace(",", ""),
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
                Price = basketItem.BasketItemTotalPrice.ToString(),
            });
        }
    }
}