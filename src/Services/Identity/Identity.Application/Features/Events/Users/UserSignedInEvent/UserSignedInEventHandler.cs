using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;

namespace Identity.Application.Features.Events.Users.UserSignedInEvent
{
    public class UserSignedInEventHandler : INotificationHandler<UserSignedInEvent>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMailService _mailService;

        public UserSignedInEventHandler(UserManager<User> userManager, IMailService mailService)
        {
            _userManager = userManager;
            _mailService = mailService;
        }


        [ExceptionLogAspect(typeof(FileLogger), "Identity-Service")]
        [LogAspect(typeof(FileLogger), "Identity-Service")]
        public async Task Handle(UserSignedInEvent notification, CancellationToken cancellationToken)
        {
            notification.User.LastLoginIp = notification.IpAddress;
            notification.User.LastLoginDate = DateTime.Now;
            await _userManager.UpdateAsync(notification.User);

            var emailTemplatePath = Path.Combine(Environment.CurrentDirectory,
               @"MailTemplates\SendLastLoginInfoEmailTemplate.html");
            using var reader = new StreamReader(emailTemplatePath);
            var mailTemplate = await reader.ReadToEndAsync();
            reader.Close();
            mailTemplate = notification.User.Gender switch
            {
                Gender.Female => mailTemplate.Replace("[Gender]", "Ms."),
                Gender.Male => mailTemplate.Replace("[Gender]", "Mr."),
                Gender.Other => mailTemplate.Replace("[Gender]", "Mx."),
                _ => mailTemplate
            };
            mailTemplate = mailTemplate.Replace("[FullName]", notification.User.FirstName.ToUpper() + " " + notification.User.LastName.ToUpper());
            mailTemplate = mailTemplate.Replace("[LoginDate]", notification.User.LastLoginDate?.ToString("MM/dd/yyyy HH:mm"));
            mailTemplate = mailTemplate.Replace("[IpAddress]", notification.User.LastLoginIp);

            await _mailService.SendMail(mailTemplate, "E-Commerce Hesabınıza Giriş Yapıldı", notification.User.Email);
        }
    }
}