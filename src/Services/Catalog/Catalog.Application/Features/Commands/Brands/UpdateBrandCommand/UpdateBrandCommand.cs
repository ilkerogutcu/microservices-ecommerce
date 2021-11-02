using Catalog.Application.Dtos;
using Catalog.Domain.Entities;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Brands.UpdateBrandCommand
{
    public class UpdateBrandCommand : IRequest<IDataResult<Brand>>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}