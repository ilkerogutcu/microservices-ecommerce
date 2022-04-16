using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventBus.Base.Abstraction;
using MediatR;
using Microsoft.Extensions.Configuration;
using Olcsan.Boilerplate.Utilities.Results;
using Order.Application.Dtos;
using Order.Application.IntegrationEvents;
using Order.Application.Interfaces.Repositories;
using Order.Application.Interfaces.Services;
using Order.Domain.AggregateModels.BuyerAggregate;
using Order.Domain.AggregateModels.OrderAggregate;
using Payment.Grpc.Protos;
using Serilog;

namespace Order.Application.Features.Commands.Orders.CreateOrderCommand
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, IResult>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IIdentityService _identityService;
        private readonly IEventBus _eventBus;
        private readonly ICatalogService _catalogService;
        private readonly IPaymentService _paymentService;
        private readonly IBuyerRepository _buyerRepository;
        private readonly IConfiguration _configuration;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IEventBus eventBus, IIdentityService identityService,
            ICatalogService catalogService, IPaymentService paymentService, IBuyerRepository buyerRepository, IConfiguration configuration)
        {
            _orderRepository = orderRepository;
            _eventBus = eventBus;
            _identityService = identityService;
            _catalogService = catalogService;
            _paymentService = paymentService;
            _buyerRepository = buyerRepository;
            _configuration = configuration;
        }

        public async Task<IResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingNonPaidOrder =
                    await _orderRepository.GetAsync(x => x.Buyer.UserId.Equals(request.UserId) && x.OrderStatus.Equals(OrderStatus.WaitingForPayment),
                        x => x.Buyer);
                if (existingNonPaidOrder != null)
                {
                    existingNonPaidOrder.orderStatusId = OrderStatus.Cancelled.Id;
                    _orderRepository.Update(existingNonPaidOrder);
                    await _orderRepository.SaveChangesAsync();
                }

                var address = new Address(request.FirstName, request.LastName, request.PhoneNumber, request.City, request.ZipCode, request.District,
                    request.AddressLine, request.AddressTitle);

                var order = new Domain.AggregateModels.OrderAggregate.Order(request.Email, request.FirstName, request.LastName, request.UserId,
                    address, request.CardTypeId, request.CardNumber, request.CardSecurityNumber, request.CardHolderName, request.CardExpirationMonth,
                    request.CardExpirationYear, null);

                request.OrderItems.ToList().ForEach(x => { order.AddOrderItem(x.ProductId, x.ProductName, x.PictureUrl, x.UnitPrice, x.Units); });


                var orderStartedIntegrationEvent = new OrderStartedIntegrationEvent(request.Email, request.FirstName, request.LastName);
                _eventBus.Publish(orderStartedIntegrationEvent);
                var basketItemListToPayment = new List<BasketItem>();

                foreach (var orderItemDto in request.OrderItems)
                {
                    var product = await _catalogService.GetProductDetailsByIdAsync(orderItemDto.ProductId);
                    await UpdateStockQuantityOfProductAsync(product, orderItemDto);
                    basketItemListToPayment.Add(new BasketItem()
                    {
                        ProductCategory = product.CategoryName,
                        ProductId = product.Id,
                        ProductName = product.Name,
                        BasketItemTotalPrice = (int) orderItemDto.UnitPrice * orderItemDto.Units,
                    });
                }

                var buyer = await CreateOrUpdateBuyerAsync(request, order);
                var totalPrice = (request.OrderItems.Sum(x => (int) x.UnitPrice * x.Units)).ToString();
                var createPaymentResponse = await CreateThreeDPaymentAsync(request, totalPrice, basketItemListToPayment, buyer, order);
                if (createPaymentResponse.Status == "success")
                {
                    order.PaymentHtmlContent = createPaymentResponse.HtmlContent;
                    await _orderRepository.AddAsync(order);
                    await _orderRepository.SaveChangesAsync();
                    return new SuccessResult("Ödeme başarılı");
                }

                return new ErrorResult("Ödeme başarısız");
            }
            catch (Exception e)
            {
                Log.Error(e, "Error while creating order");
                return new ErrorResult("Error while creating order");
            }
        }

        private async Task UpdateStockQuantityOfProductAsync(ProductDetailsDto product, OrderItemDto orderItemDto)
        {
            product.StockQuantity -= orderItemDto.Units;
            await _catalogService.UpdateProductStockQuantityById(product.Id, product.StockQuantity);
        }

        private async Task<CreatePaymentResponse> CreateThreeDPaymentAsync(CreateOrderCommand request, string totalPrice,
            List<BasketItem> basketItemListToPayment, Buyer buyer, Domain.AggregateModels.OrderAggregate.Order order)
        {
            return await _paymentService.CreateThreeDPaymentAsync(new PaymentRequest()
            {
                BuyerEmailAddress = request.Email,
                BuyerCity = request.City,
                CardNumber = request.CardNumber,
                CardHolderName = request.CardHolderName,
                Cvc = request.CardSecurityNumber,
                ExpireMonth = request.CardExpirationMonth,
                ExpireYear = request.CardExpirationYear,
                Price = totalPrice,
                BasketItems = new BasketItemList {BasketItem = {basketItemListToPayment}},
                BuyerCountry = "Turkey",
                BuyerId = buyer.Id.ToString(),
                BuyerIp = "192.168.1.1",
                BuyerName = request.FirstName,
                BuyerSurname = request.LastName,
                ConservationId = order.Id.ToString(),
                PaidPrice = totalPrice,
                ShippingAddress = request.AddressLine,
                BuyerGsmNumber = "53212345678",
                BuyerIdentityNumber = "12345678901",
                BuyerRegistrationAddress = request.AddressLine,
                BuyerZipCode = request.ZipCode,
                BuyerRegistrationDate = buyer.CreatedDate.ToString("yyyy-MM-dd HH':'mm':'ss"),
                BuyerLastLoginDate = buyer.CreatedDate.ToString("yyyy-MM-dd HH':'mm':'ss"),
                CallbackUrl = _configuration["IyzicoSettings:CallbackUrl"],
            });
        }

        private async Task<Buyer> CreateOrUpdateBuyerAsync(CreateOrderCommand request, Domain.AggregateModels.OrderAggregate.Order order)
        {
            var buyer = await _buyerRepository.GetAsync(x => x.UserId == request.UserId);

            var buyerExisted = buyer != null;
            if (!buyerExisted)
            {
                buyer = new Buyer(request.Email, request.UserId, request.FirstName, request.LastName);
            }

            buyer.VerifyBuyerMethod(order.Id);
            var result = buyerExisted ? _buyerRepository.Update(buyer) : await _buyerRepository.AddAsync(buyer);
            await _buyerRepository.SaveChangesAsync();
            return result;
        }
    }
}