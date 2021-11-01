using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Wrappers;
using MediatR;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.Brands.GetActiveBrandsQuery
{
    public class GetActiveBrandsQueryHandler : IRequestHandler<GetActiveBrandsQuery, IDataResult<List<BrandDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IBrandRepository _brandRepository;

        public GetActiveBrandsQueryHandler(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        public async Task<IDataResult<List<BrandDto>>> Handle(GetActiveBrandsQuery request,
            CancellationToken cancellationToken)
        {
            var brands = await _brandRepository.GetListAsync(x => x.IsActive);

            var result = _mapper.Map<IEnumerable<BrandDto>>(brands)
                .OrderBy(x => x.Name).Skip(request.PageSize * request.PageIndex)
                .Take(request.PageSize).ToList();
            return new PaginatedResult<List<BrandDto>>(result, request.PageIndex, request.PageSize, brands.Count());
        }
    }
}