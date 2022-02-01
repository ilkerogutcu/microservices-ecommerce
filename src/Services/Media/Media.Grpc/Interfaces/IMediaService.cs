using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Media.Grpc.Interfaces
{
    public interface IMediaService
    {
         Task<Entities.Media> UploadImageAsync(IFormFile file);
    }
}