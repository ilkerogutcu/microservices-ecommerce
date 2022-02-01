using System.Threading.Tasks;
using AutoMapper;
using Catalog.Application.Extensions;
using Catalog.Application.Interfaces;
using Google.Protobuf;
using Media.Grpc.Protos;
using Microsoft.AspNetCore.Http;

namespace Catalog.Infrastructure.GrpcServices
{
    public class MediaGrpcService : IMediaGrpcService
    {
        private readonly IMapper _mapper;
        private readonly MediaProtoService.MediaProtoServiceClient _mediaProtoService;

        public MediaGrpcService(MediaProtoService.MediaProtoServiceClient mediaProtoService, IMapper mapper)
        {
            _mediaProtoService = mediaProtoService;
            _mapper = mapper;
        }

        public async Task<Domain.Entities.Media> UploadImage(IFormFile file)
        {
            var uploadMediaRequest = new UploadMediaRequest()
            {
                Media = ByteString.CopyFrom(await file.GetBytesAsync())
            };

            var mediaModel = _mediaProtoService.UploadImage(uploadMediaRequest);
            var media = _mapper.Map<Domain.Entities.Media>(mediaModel.MediaModel);
            media.CreatedDate = mediaModel.MediaModel.CreatedTimestamp.ToDateTime();

            return media;
        }
    }
}