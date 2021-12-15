using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;

namespace Identity.Application.Features.Events.Users.SendEmailTwoFactorTokenEvent
{
    public class SendEmailTwoFactorTokenEventHandler : INotificationHandler<SendEmailTwoFactorTokenEvent>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMailService _mailService;

        public SendEmailTwoFactorTokenEventHandler(UserManager<User> userManager, IMailService mailService)
        {
            _userManager = userManager;
            _mailService = mailService;
        }
        [ExceptionLogAspect(typeof(FileLogger), "Identity-Service")]
        [LogAspect(typeof(FileLogger), "Identity-Service")]
        public async Task Handle(SendEmailTwoFactorTokenEvent notification, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(notification.UserId);
            if (user is null)
            {
                return;
            }

            var code = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            var emailTemplatePath = Path.Combine(Environment.CurrentDirectory,
                @"MailTemplates\SendTwoFactorTokenEmailTemplate.html");
            using var reader = new StreamReader(emailTemplatePath);
            var mailTemplate = await reader.ReadToEndAsync();
            reader.Close();
            await _mailService.SendMail(mailTemplate.Replace("[2FACode]", code), $"Your code is {code}", user.Email);
        }
    }
}