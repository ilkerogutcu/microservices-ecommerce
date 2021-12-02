using Catalog.Domain.Entities;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Categories.CreateCategoryCommand
{
    public class CreateCategoryCommand : IRequest<IDataResult<Category>>
    {
        public string MainCategoryId { get; set; }
        public string SubCategoryId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}