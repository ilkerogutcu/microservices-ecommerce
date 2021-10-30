using Catalog.Application.Dtos;
using Catalog.Domain.Entities;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.UpdateBrandCommand
{
    public class UpdateBrandCommand : IRequest<IDataResult<BrandDto>>
    {
        public UpdateBrandCommand(BrandDto brandDto)
        {
            BrandDto = brandDto;
        }

        public BrandDto BrandDto { get; }
    }
}