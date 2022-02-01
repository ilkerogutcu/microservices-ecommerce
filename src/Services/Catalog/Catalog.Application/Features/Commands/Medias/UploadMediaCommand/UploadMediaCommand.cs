using MediatR;
using Microsoft.AspNetCore.Http;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Medias.UploadMediaCommand
{
    public class UploadMediaCommand: IRequest<IDataResult<Domain.Entities.Media>>
    {
        public IFormFile Media { get; set; }
    }
}