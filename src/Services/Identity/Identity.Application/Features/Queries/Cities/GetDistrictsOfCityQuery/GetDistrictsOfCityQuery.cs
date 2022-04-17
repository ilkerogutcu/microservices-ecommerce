using System;
using System.Collections.Generic;
using Identity.Application.Features.Queries.Cities.ViewModels;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Queries.Cities.GetDistrictsOfCityQuery
{
    public class GetDistrictsOfCityQuery : IRequest<IDataResult<List<DistrictViewModel>>>
    {
        public Guid CityId { get; set; }

        public GetDistrictsOfCityQuery(Guid cityId)
        {
            CityId = cityId;
        }
    }
}