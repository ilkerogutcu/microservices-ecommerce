using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Interfaces
{
    public interface IMediaGrpcService
    {
        Task<List<Domain.Entities.Media>> UploadImage(List<IFormFile> mediaList);
    }
}