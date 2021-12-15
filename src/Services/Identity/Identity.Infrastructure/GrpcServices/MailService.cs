using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf;
using Identity.Application.Extensions;
using Identity.Application.Interfaces;
using Mail.Grpc.Protos;
using Microsoft.AspNetCore.Http;

namespace Identity.Infrastructure.GrpcServices
{
    public class MailService : IMailService
    {
        private readonly MailProtoService.MailProtoServiceClient _mailServiceClient;

        public MailService(MailProtoService.MailProtoServiceClient mailServiceClient)
        {
            _mailServiceClient = mailServiceClient;
        }

        public async Task SendMail(string body, string subject, string toEmail, List<IFormFile> attachments = null)
        {
            var sendEmailRequest = new SendEmailRequest
            {
                Body = body,
                Subject = subject,
                ToEmail = toEmail
            };
            if (attachments is not null)
            {
                foreach (var attachment in attachments)
                {
                    sendEmailRequest.Attachments.Add(ByteString.CopyFrom(await attachment.GetBytesAsync()));
                }
            }

            _mailServiceClient.SendEmail(sendEmailRequest);
        }
    }
}