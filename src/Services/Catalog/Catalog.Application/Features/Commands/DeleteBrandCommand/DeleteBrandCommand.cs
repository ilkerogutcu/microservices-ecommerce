using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.DeleteBrandCommand
{
    public class DeleteBrandCommand: IRequest<IResult>
    {
        public string Id { get; set; }
    }
}