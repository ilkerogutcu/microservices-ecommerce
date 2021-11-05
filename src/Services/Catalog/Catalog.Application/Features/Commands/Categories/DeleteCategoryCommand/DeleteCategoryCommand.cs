using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Categories.DeleteCategoryCommand
{
    public class DeleteCategoryCommand : IRequest<IResult>
    {
        public string MainCategoryId { get; set; }
        public string SubCategoryId { get; set; }
        public string ParentId { get; set; }
    }
}