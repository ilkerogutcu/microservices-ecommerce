using System.IO;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Mail.Grpc.Configs;
using Mail.Grpc.Protos;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Mail.Grpc.Services
{
    public class MailService : MailProtoService.MailProtoServiceBase
    {
        private readonly IConfiguration _configuration;
        private readonly MailConfigs _mailConfigs;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _mailConfigs = configuration.GetSection("EmailConfigs").Get<MailConfigs>();
        }

        public override async Task<Empty> SendEmail(SendEmailRequest request,
            ServerCallContext context)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailConfigs.Mail),
                Subject = request.Subject,
                To = {MailboxAddress.Parse(request.ToEmail)},
            };
            var builder = new BodyBuilder();
            if (request.Attachments != null)
            {
                foreach (var fileByteString in request.Attachments)
                {
                    var byteStringToArray = fileByteString.ToByteArray();
                    var stream = new MemoryStream(byteStringToArray);
                    var file = new FormFile(stream, 0, byteStringToArray.Length, "name", "fileName");
                    byte[] fileBytes;
                    await using (var ms = new MemoryStream())
                    {
                        await file.CopyToAsync(ms);
                        fileBytes = ms.ToArray();
                    }

                    builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                }
            }

            builder.HtmlBody = request.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailConfigs.Host, _mailConfigs.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailConfigs.Mail, _mailConfigs.Password);
            var result=await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
            return new Empty();
        }
    }
}