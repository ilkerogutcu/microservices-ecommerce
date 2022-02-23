﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventBus.Base.Abstraction;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;
using Order.Application.IntegrationEvents;
using Order.Application.Interfaces.Repositories;
using Order.Application.Interfaces.Services;
using Order.Domain.AggregateModels.OrderAggregate;
using Serilog;

namespace Order.Application.Features.Commands.Orders.CreateOrderCommand
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, IResult>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IIdentityService _identityService;
        private readonly IEventBus _eventBus;
        private readonly ICatalogService _catalogService;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IEventBus eventBus, IIdentityService identityService,
            ICatalogService catalogService)
        {
            _orderRepository = orderRepository;
            _eventBus = eventBus;
            _identityService = identityService;
            _catalogService = catalogService;
        }

        public async Task<IResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var address = new Address(request.FirstName, request.LastName, request.PhoneNumber, request.City, request.ZipCode, request.District,
                    request.AddressLine, request.AddressTitle);

                var order = new Domain.AggregateModels.OrderAggregate.Order(request.Email, request.FirstName, request.LastName, request.UserId,
                    address, request.CardTypeId, request.CardNumber, request.CardSecurityNumber, request.CardHolderName, request.CardExpiration,
                    null);

                request.OrderItems.ToList().ForEach(x => { order.AddOrderItem(x.ProductId, x.ProductName, x.PictureUrl, x.UnitPrice, x.Units); });

                await _orderRepository.AddAsync(order);
                await _orderRepository.SaveChangesAsync();
                var orderStartedIntegrationEvent = new OrderStartedIntegrationEvent(request.Email, request.FirstName, request.LastName);
                _eventBus.Publish(orderStartedIntegrationEvent);

                request.OrderItems.ToList().ForEach(async x =>
                {
                    var product = await _catalogService.GetProductDetailsByIdAsync(x.ProductId);
                    product.StockQuantity -= x.Units;
                    await _catalogService.UpdateProductStockQuantityById(product.Id, product.StockQuantity);
                });
                return new SuccessResult();
            }
            catch (Exception e)
            {
                Log.Error(e, "Error while creating order");
                return new ErrorResult("Error while creating order");
            }
        }
    }
}