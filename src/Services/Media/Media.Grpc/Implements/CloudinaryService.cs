using System;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Media.Grpc.Configs;
using Media.Grpc.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Media.Grpc.Implements
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IConfiguration _configuration;
        public CloudinaryService(IConfiguration configuration)
        {
            _configuration = configuration;
            var cloudinarySettings = configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();
            var account = new Account(
                cloudinarySettings.CloudName,
                cloudinarySettings.ApiKey,
                cloudinarySettings.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<Entities.Media> UploadImageAsync(IFormFile file)
        {
            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.Name, stream)
            };
            var uploadResult = _cloudinary.Upload(uploadParams);
            return new Entities.Media
            {
                Url = uploadResult.Url.ToString(),
                PublicId = uploadResult.PublicId
            };
        }
    }
}