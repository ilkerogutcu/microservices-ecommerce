using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Wrappers;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.OptionValues.GetAllOptionValuesQuery
{
    public class
        GetAllOptionValuesQueryHandler : IRequestHandler<GetAllOptionValuesQuery, IDataResult<List<OptionValueDetailsDto>>>
    {
        private readonly IOptionValueRepository _optionValueRepository;

        public GetAllOptionValuesQueryHandler(IOptionValueRepository optionValueRepository)
        {
            _optionValueRepository = optionValueRepository;
        }

        public async Task<IDataResult<List<OptionValueDetailsDto>>> Handle(GetAllOptionValuesQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _optionValueRepository.GetAllDetailsAsync();
            return new SuccessDataResult<List<OptionValueDetailsDto>>(result);
        }
    }
}