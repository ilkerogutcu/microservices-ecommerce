using Catalog.Domain.Entities;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.OptionValues.UpdateOptionValueCommand
{
    public class UpdateOptionValueCommand : IRequest<IDataResult<OptionValue>>
    {
        public string Id { get; set; }
        public string OptionId { get; set; }
        public string Name { get; set; }
    }
}