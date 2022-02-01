using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.IoC;

namespace Identity.Application.Features.Events.Users.SendEmailConfirmationTokenEvent
{
    public class SendEmailConfirmationTokenEventHandler : INotificationHandler<SendEmailConfirmationTokenEvent>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMailService _mailService;
        private readonly string _baseUrl;
        
        public SendEmailConfirmationTokenEventHandler(IMailService mailService, UserManager<User> userManager)
        {
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
            _baseUrl = configuration.GetSection("BaseUrl").Value;
            _mailService = mailService;
            _userManager = userManager;
        }

        [LogAspect(typeof(FileLogger), "Identity-Service")]
        [ExceptionLogAspect(typeof(FileLogger), "Identity-Service")]
        public async Task Handle(SendEmailConfirmationTokenEvent notification, CancellationToken cancellationToken)
        {
            // Generate token for confirm email
            var user = await _userManager.FindByIdAsync(notification.UserId);
            if (user is null)
            {
                return;
            }

            var verificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var encodedVerificationToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(verificationToken));

            // Generate endpoint url for verification url
            var endPointUrl = new Uri(string.Concat($"{_baseUrl}", "api/v1/accounts/confirm-email/"));
            var verificationUrl = QueryHelpers.AddQueryString(endPointUrl.ToString(), "userId", notification.UserId);
            verificationUrl = QueryHelpers.AddQueryString(verificationUrl, "verificationToken", encodedVerificationToken);
            
            var emailTemplatePath = Path.Combine(Environment.CurrentDirectory,
                @"MailTemplates\SendVerificationEmailTemplate.html");
            using var reader = new StreamReader(emailTemplatePath);
            var mailTemplate = await reader.ReadToEndAsync();
            reader.Close();
            await _mailService.SendMail(mailTemplate.Replace("[verificationUrl]", verificationUrl),
                "Please verification your email", user.Email, new List<IFormFile>());
        }
    }
}