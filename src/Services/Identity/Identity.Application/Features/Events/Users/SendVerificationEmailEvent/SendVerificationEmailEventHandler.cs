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

namespace Identity.Application.Features.Events.Users.SendVerificationEmailEvent
{
    public class SendVerificationEmailEventHandler : INotificationHandler<SendVerificationEmailEvent>
    {
        private readonly IMailService _mailService;
        private readonly string _baseUrl;
        
        public SendVerificationEmailEventHandler(IMailService mailService)
        {
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
            _baseUrl = configuration.GetSection("BaseUrl").Value;
            _mailService = mailService;
        }

        public async Task Handle(SendVerificationEmailEvent notification, CancellationToken cancellationToken)
        {
            // Generate token for confirm email
            var verificationToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(notification.VerificationToken));

            // Generate endpoint url for verification url
            var endPointUrl = new Uri(string.Concat($"{_baseUrl}", "api/account/confirm-email/"));
            var verificationUrl = QueryHelpers.AddQueryString(endPointUrl.ToString(), "userId", notification.User.Id);
            verificationUrl = QueryHelpers.AddQueryString(verificationUrl, "verificationToken", verificationToken);
            
            var emailTemplatePath = Path.Combine(Environment.CurrentDirectory,
                @"MailTemplates\SendVerificationEmailTemplate.html");
            using var reader = new StreamReader(emailTemplatePath);
            var mailTemplate = await reader.ReadToEndAsync();
            reader.Close();
            await _mailService.SendMail(mailTemplate.Replace("[verificationUrl]", verificationUrl),
                "Please verification your email", notification.User.Email, new List<IFormFile>());
        }
    }
}