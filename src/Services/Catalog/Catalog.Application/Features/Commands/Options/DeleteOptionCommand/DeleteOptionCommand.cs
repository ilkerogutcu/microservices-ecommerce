using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Options.DeleteOptionCommand
{
    public class DeleteOptionCommand : IRequest<IResult>
    {
        public string Id { get; set; }
    }
}