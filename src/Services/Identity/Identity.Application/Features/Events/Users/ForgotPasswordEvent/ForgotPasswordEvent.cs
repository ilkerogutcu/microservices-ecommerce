using MediatR;

namespace Identity.Application.Features.Events.Users.ForgotPasswordEvent
{
    public class ForgotPasswordEvent : INotification
    {
        public ForgotPasswordEvent(string resetToken, string userId, string email)
        {
            ResetToken = resetToken;
            UserId = userId;
            Email = email;
        }

        public string ResetToken { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
    }
}