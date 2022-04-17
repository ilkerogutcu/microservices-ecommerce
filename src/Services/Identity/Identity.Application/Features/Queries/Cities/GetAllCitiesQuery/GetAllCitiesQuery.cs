using System.Collections.Generic;
using Identity.Application.Features.Queries.Cities.ViewModels;
using Identity.Domain.Entities;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Queries.Cities.GetAllCitiesQuery
{
    public class GetAllCitiesQuery : IRequest<IDataResult<List<CityViewModel>>>
    {
    }
}