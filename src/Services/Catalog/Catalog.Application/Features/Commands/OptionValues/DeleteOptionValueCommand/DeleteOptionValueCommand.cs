using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.OptionValues.DeleteOptionValueCommand
{
    public class DeleteOptionValueCommand : IRequest<IResult>
    {
        public string Id { get; set; }
    }
}