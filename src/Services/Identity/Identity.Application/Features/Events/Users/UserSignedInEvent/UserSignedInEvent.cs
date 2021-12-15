using Identity.Domain.Entities;
using MediatR;

namespace Identity.Application.Features.Events.Users.UserSignedInEvent
{
    public class UserSignedInEvent : INotification
    {
        public UserSignedInEvent(string ipAddress, User user)
        {
            IpAddress = ipAddress;
            User = user;
        }

        public string IpAddress { get; }
        public User User { get; }
    }
}