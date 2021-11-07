using System.Collections.Generic;
using Catalog.Domain.Entities;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Categories.CreateCategoryOptionValueCommand
{
    public class CreateCategoryOptionValueCommand: IRequest<IDataResult<CategoryOptionValue>>
    {
        public string CategoryId { get; set; }
        public List<string>  OptionValueIds { get; set; }
    }
}