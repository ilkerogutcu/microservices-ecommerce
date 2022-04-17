using Catalog.Domain.Entities;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Options.UpdateOptionCommand
{
    public class UpdateOptionCommand: IRequest<IDataResult<Option>>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsRequired { get; set; }
        public bool Varianter { get; set; }
        public bool IsActive { get; set; }
    }
}