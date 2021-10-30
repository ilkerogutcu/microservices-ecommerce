using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.CreateBrandCommand
{
    public class CreateBrandCommand : IRequest<IResult>
    {

        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}