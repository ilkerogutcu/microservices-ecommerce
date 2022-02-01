using Catalog.Domain.Entities;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Options.CreateOptionCommand
{
    public class CreateOptionCommand : IRequest<IDataResult<Option>>
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}