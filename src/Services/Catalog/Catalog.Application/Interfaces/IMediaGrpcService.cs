using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Interfaces
{
    public interface IMediaGrpcService
    {
        Task<Domain.Entities.Media> UploadImage(IFormFile file);
    }
}