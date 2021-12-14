using Identity.Domain.Entities;
using MediatR;

namespace Identity.Application.Features.Events.Users.SendVerificationEmailEvent
{
    public class SendVerificationEmailEvent : INotification
    {
        public SendVerificationEmailEvent(User user, string verificationToken)
        {
            User = user;
            VerificationToken = verificationToken;
        }

        public User User { get; set; }
        public string VerificationToken { get; set; }
    }
}