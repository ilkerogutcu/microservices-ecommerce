using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Identity.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Olcsan.Boilerplate.Utilities.IoC;

namespace Identity.Application.Features.Events.Users.ForgotPasswordEvent
{
    public class ForgotPasswordEventHandler: INotificationHandler<ForgotPasswordEvent>
    {
        private readonly IMailService _mailService;
        private readonly string _baseUrl;

        public ForgotPasswordEventHandler(IMailService mailService)
        {
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
            _mailService = mailService;
            _baseUrl = configuration.GetSection("BaseUrl").Value;

        }

        public async Task Handle(ForgotPasswordEvent notification, CancellationToken cancellationToken)
        {
            var encodedResetToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(notification.ResetToken));
            var endPointUrl = new Uri(string.Concat($"{_baseUrl}", "api/account/reset-password/"));
          
            var resetTokenUrl = QueryHelpers.AddQueryString(endPointUrl.ToString(), "userId",  notification.UserId);
            resetTokenUrl = QueryHelpers.AddQueryString(resetTokenUrl, "token", encodedResetToken);
            // Edit forgot password email template for reset password link
            var emailTemplatePath = Path.Combine(Environment.CurrentDirectory,
                @"MailTemplates\SendForgotPasswordEmailTemplate.html");
            using var reader = new StreamReader(emailTemplatePath);
            var mailTemplate = await reader.ReadToEndAsync();
            reader.Close();
            await _mailService.SendMail(mailTemplate.Replace("[resetPasswordLink]", resetTokenUrl),
                "You have requested to reset your password", notification.Email, new List<IFormFile>());
        }
    }
}