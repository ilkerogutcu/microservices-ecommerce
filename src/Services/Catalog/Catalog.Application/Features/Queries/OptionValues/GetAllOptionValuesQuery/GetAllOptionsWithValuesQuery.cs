using System;
using System.Collections.Generic;
using Catalog.Application.Dtos;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.OptionValues.GetAllOptionValuesQuery
{
    public class GetAllOptionsWithValuesQuery : IRequest<IDataResult<List<OptionWithValuesDto>>>
    {
    }
}