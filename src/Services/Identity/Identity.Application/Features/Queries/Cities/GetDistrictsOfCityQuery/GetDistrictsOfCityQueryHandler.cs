using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Identity.Application.Features.Queries.Cities.ViewModels;
using Identity.Application.Interfaces.Repositories;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Identity.Application.Features.Queries.Cities.GetDistrictsOfCityQuery
{
    public class GetDistrictsOfCityQueryHandler : IRequestHandler<GetDistrictsOfCityQuery, IDataResult<List<DistrictViewModel>>>
    {
        private readonly IDistrictRepository _districtRepository;
        private readonly IMapper _mapper;

        public GetDistrictsOfCityQueryHandler(IDistrictRepository districtRepository, IMapper mapper)
        {
            _districtRepository = districtRepository;
            _mapper = mapper;
        }

        public async Task<IDataResult<List<DistrictViewModel>>> Handle(GetDistrictsOfCityQuery request, CancellationToken cancellationToken)
        {
            var districts = await _districtRepository.GetListAsync(x => x.CityId.Equals(request.CityId));
            var result = _mapper.Map<List<DistrictViewModel>>(districts).OrderBy(x=>x.Name).ToList();;
            return new SuccessDataResult<List<DistrictViewModel>>(result);
        }
    }
}