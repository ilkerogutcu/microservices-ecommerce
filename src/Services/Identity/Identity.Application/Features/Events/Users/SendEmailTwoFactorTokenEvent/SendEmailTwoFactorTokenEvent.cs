using MediatR;

namespace Identity.Application.Features.Events.Users.SendEmailTwoFactorTokenEvent
{
    public class SendEmailTwoFactorTokenEvent : INotification
    {
        public SendEmailTwoFactorTokenEvent(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
    }
}