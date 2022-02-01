using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Categories.DeleteCategoryOptionValueCommand
{
    public class DeleteCategoryOptionValueCommand: IRequest<IResult>
    {
        public string Id { get; set; }
    }
}