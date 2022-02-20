using System;
using System.Collections.Generic;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;
using Order.Application.Dtos;
using Order.Domain.Models;

namespace Order.Application.Features.Commands.Orders.CreateOrderCommand
{
    public class CreateOrderCommand : IRequest<IResult>
    {
        private readonly List<OrderItemDto> _orderItems;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; }
        public string City { get; }
        public string ZipCode { get; set; }
        public string District { get; }
        public string AddressLine { get; }
        public string AddressTitle { get; set; }
        public string CardNumber { get; }
        public string CardHolderName { get; }
        public DateTime CardExpiration { get; }
        public string CardSecurityNumber { get; }
        public int CardTypeId { get; }
        public IEnumerable<OrderItemDto> OrderItems => _orderItems;

        public CreateOrderCommand()
        {
            _orderItems = new List<OrderItemDto>();
        }

        public CreateOrderCommand(List<BasketItem> basketItems, string firstName, string lastName, string phoneNumber, string email, string city,
            string district, string zipCode, string addressLine, string addressTitle, string cardNumber,
            string cardHolderName, DateTime cardExpiration, string cardSecurityNumber, int cardTypeId) : this()
        {
            foreach (var item in basketItems)
            {
                _orderItems.Add(new OrderItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    PictureUrl = item.PictureUrl,
                    UnitPrice = item.UnitPrice,
                    Units = item.Quantity
                });
            }

            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            City = city;
            District = district;
            ZipCode = zipCode;
            AddressLine = addressLine;
            AddressTitle = addressTitle;
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            CardExpiration = cardExpiration;
            CardSecurityNumber = cardSecurityNumber;
            CardTypeId = cardTypeId;
        }
    }
}