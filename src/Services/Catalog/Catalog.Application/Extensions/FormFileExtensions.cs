using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Extensions
{
    public static  class FormFileExtensions
    {
        public static async Task<byte[]> GetBytesAsync(this IFormFile formFile)
        {
            await using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}