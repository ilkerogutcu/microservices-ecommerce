using System;
using MediatR;

namespace Identity.Application.Features.Events.Users.SendEmailConfirmationTokenEvent
{
    public class SendEmailConfirmationTokenEvent : INotification
    {
        public SendEmailConfirmationTokenEvent(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; set; }
    }
}