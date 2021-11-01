using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Wrappers;
using Catalog.Domain.Entities;
using MediatR;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.Options.GetAllOptionsQuery
{
    public class GetAllOptionsQueryHandler : IRequestHandler<GetAllOptionsQuery, IDataResult<List<OptionDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IOptionRepository _optionRepository;

        public GetAllOptionsQueryHandler(IMapper mapper, IOptionRepository optionRepository)
        {
            _mapper = mapper;
            _optionRepository = optionRepository;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        public async Task<IDataResult<List<OptionDto>>> Handle(GetAllOptionsQuery request,
            CancellationToken cancellationToken)
        {
            var options = await _optionRepository.GetListAsync();
            if (request.Varianter is not null)
            {
                options = options.Where(x => x.Varianter == request.Varianter);
            }

            if (request.IsActive is not null)
            {
                options = options.Where(x => x.IsActive == request.IsActive);
            }

            if (request.IsRequired is not null)
            {
                options = options.Where(x => x.IsRequired == request.IsRequired);
            }

            var result = _mapper.Map<IEnumerable<OptionDto>>(options)
                .OrderBy(x => x.Name)
                .Skip(request.PageSize * request.PageIndex)
                .Take(request.PageSize).ToList();
            return new PaginatedResult<List<OptionDto>>(result, request.PageIndex, request.PageSize, result.Count);
        }
    }
}