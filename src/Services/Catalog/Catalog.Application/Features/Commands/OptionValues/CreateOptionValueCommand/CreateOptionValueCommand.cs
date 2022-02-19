using Catalog.Domain.Entities;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.OptionValues.CreateOptionValueCommand
{
    public class CreateOptionValueCommand : IRequest<IDataResult<OptionValue>>
    {
        public string OptionId { get; set; }
        public string Name { get; set; }
   
    }
}