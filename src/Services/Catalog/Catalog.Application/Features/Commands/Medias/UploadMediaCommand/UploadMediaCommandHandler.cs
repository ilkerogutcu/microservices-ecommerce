using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Constants;
using Catalog.Application.Interfaces;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Medias.UploadMediaCommand
{
    public class UploadMediaCommandHandler : IRequestHandler<UploadMediaCommand, IDataResult<Domain.Entities.Media>>
    {
        private readonly IMediaGrpcService _mediaGrpcService;

        public UploadMediaCommandHandler(IMediaGrpcService mediaGrpcService)
        {
            _mediaGrpcService = mediaGrpcService;
        }

        public async Task<IDataResult<Domain.Entities.Media>> Handle(UploadMediaCommand request,
            CancellationToken cancellationToken)
        {
            var media = await _mediaGrpcService.UploadImage(request.Media);
            if (media is null)
            {
                return new ErrorDataResult<Domain.Entities.Media>(Messages.ErrorWhileUploadingMedia);
            }

            return new SuccessDataResult<Domain.Entities.Media>(media);
        }
    }
}