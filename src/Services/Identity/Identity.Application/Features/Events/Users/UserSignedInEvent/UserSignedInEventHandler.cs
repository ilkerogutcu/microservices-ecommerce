using System;
using System.Threading;
using System.Threading.Tasks;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Features.Events.Users.UserSignedInEvent
{
    public class UserSignedInEventHandler : INotificationHandler<UserSignedInEvent>
    {
        private readonly UserManager<User> _userManager;

        public UserSignedInEventHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(UserSignedInEvent notification, CancellationToken cancellationToken)
        {
            notification.User.LastLoginIp = notification.IpAddress;
            notification.User.LastLoginDate = DateTime.Now;
            await _userManager.UpdateAsync(notification.User);
        }
    }
}