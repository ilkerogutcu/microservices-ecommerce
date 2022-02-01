using Identity.Domain.Entities;
using MediatR;

namespace Identity.Application.Features.Events.Users.UserUpdatedEvent
{
    public class UserUpdatedEvent : INotification
    {
        public UserUpdatedEvent(User user)
        {
            User = user;
        }

        public User User { get; set; }
    }
}