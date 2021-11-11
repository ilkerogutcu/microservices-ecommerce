using System.Collections.Generic;
using Catalog.Application.Dtos;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.Options.GetAllOptionsQuery
{
    public class GetAllOptionsQuery : IRequest<IDataResult<List<OptionDto>>>
    {
        public bool? IsActive { get; set; } = null;
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 0;
    }
}