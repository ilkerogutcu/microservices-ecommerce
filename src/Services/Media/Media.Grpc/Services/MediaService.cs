using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using Media.Grpc.Interfaces;
using Media.Grpc.Protos;
using Microsoft.AspNetCore.Http;

namespace Media.Grpc.Services
{
    public class MediaService : MediaProtoService.MediaProtoServiceBase
    {
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

        public MediaService(ICloudinaryService cloudinaryService, IMapper mapper)
        {
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;
        }

        public override async Task<MediaModelResponse> UploadImage(UploadMediaRequest request,
            ServerCallContext context)
        {
            var result = new MediaModelResponse();
            foreach (var mediaByteString in request.MediaList)
            {
                var byteStringToArray = mediaByteString.ToByteArray();
                var stream = new MemoryStream(byteStringToArray);
                var file = new FormFile(stream, 0, byteStringToArray.Length, "name", "fileName");
                var media = await _cloudinaryService.UploadImageAsync(file);
                result.MediaModels.Add(_mapper.Map<MediaModel>(media));
            }

            return result;
        }
    }
}