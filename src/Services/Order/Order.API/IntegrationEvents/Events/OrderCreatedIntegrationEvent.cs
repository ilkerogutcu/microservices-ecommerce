using System;
using EventBus.Base.Events;
using Order.Domain.Models;

namespace Order.API.IntegrationEvents.Events
{
    public class OrderCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Zip { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressLine { get; set; }
        public string AddressTitle { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public DateTime CardExpiration { get; set; }
        public string CardSecurityNumber { get; set; }
        public int CardTypeId { get; set; }
        public string Buyer { get; set; }
        public CustomerBasket Basket { get; set; }

        public OrderCreatedIntegrationEvent(Guid userId, string city, string district, string zip, string firstName, string lastName,
            string phoneNumber, string addressLine, string addressTitle, string cardNumber, string cardHolderName, DateTime cardExpiration,
            string cardSecurityNumber, int cardTypeId, string buyer,
            CustomerBasket basket, string email)
        {
            UserId = userId;
            City = city;
            District = district;
            Zip = zip;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            AddressLine = addressLine;
            AddressTitle = addressTitle;
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            CardExpiration = cardExpiration;
            CardSecurityNumber = cardSecurityNumber;
            CardTypeId = cardTypeId;
            Buyer = buyer;
            Basket = basket;
            Email = email;
        }
    }
}