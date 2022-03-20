using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Identity.Application.Features.Queries.Cities.ViewModels;
using Identity.Application.Interfaces.Repositories;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Queries.Cities.GetAllCitiesQuery
{
    public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, IDataResult<List<CityViewModel>>>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;

        public GetAllCitiesQueryHandler(ICityRepository cityRepository, IMapper mapper)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
        }

        public async Task<IDataResult<List<CityViewModel>>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
        {
            var cities = await _cityRepository.GetListAsync();
            var result = _mapper.Map<List<CityViewModel>>(cities).OrderBy(x=>x.Name).ToList();
            return new SuccessDataResult<List<CityViewModel>>(result);
        }
    }
}