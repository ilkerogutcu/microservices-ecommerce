using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<Domain.Entities.Media>> UploadImage(List<IFormFile> mediaList)
        {
            var uploadMediaRequest = new UploadMediaRequest();
            foreach (var media in mediaList)
            {
                uploadMediaRequest.MediaList.Add(ByteString.CopyFrom(await media.GetBytesAsync()));
            }

            var mediaModel =  _mediaProtoService.UploadImage(uploadMediaRequest);
            return _mapper.Map<List<Domain.Entities.Media>>(mediaModel.MediaModels.ToList());
        }
    }
}