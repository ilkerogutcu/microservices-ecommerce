using System;
using EventBus.Base.Events;

namespace Order.Application.IntegrationEvents
{
    public class OrderStartedIntegrationEvent : IntegrationEvent
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public OrderStartedIntegrationEvent(string email, string firstName, string lastName)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}