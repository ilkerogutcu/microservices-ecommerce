using System;
using System.Threading;
using System.Threading.Tasks;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Features.Events.Users.UserUpdatedEvent
{
    public class UserUpdatedEventHandler : INotificationHandler<UserUpdatedEvent>
    {
        private readonly UserManager<User> _userManager;

        public UserUpdatedEventHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(UserUpdatedEvent notification, CancellationToken cancellationToken)
        {
            notification.User.LastUpdatedDate = DateTime.Now;
            await _userManager.UpdateAsync(notification.User);
        }
    }
}