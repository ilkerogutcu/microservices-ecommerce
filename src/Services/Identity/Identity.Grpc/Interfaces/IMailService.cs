using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Identity.Grpc.Interfaces
{
    public interface IMailService
    {
        Task SendMail(string body, string subject, string toEmail, List<IFormFile> attachments = null);
    }
}