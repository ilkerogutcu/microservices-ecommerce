using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Wrappers;
using MediatR;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.OptionValues.GetAllOptionValuesQuery
{
    public class GetAllOptionsWithValuesQueryHandler : IRequestHandler<GetAllOptionsWithValuesQuery, IDataResult<List<OptionWithValuesDto>>>
    {
        private readonly IOptionValueRepository _optionValueRepository;

        public GetAllOptionsWithValuesQueryHandler(IOptionValueRepository optionValueRepository)
        {
            _optionValueRepository = optionValueRepository;
        }
        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        public async Task<IDataResult<List<OptionWithValuesDto>>> Handle(GetAllOptionsWithValuesQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _optionValueRepository.GetAllDetailsAsync();
            return new SuccessDataResult<List<OptionWithValuesDto>>(result);
        }
    }
}