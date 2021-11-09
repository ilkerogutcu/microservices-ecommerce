using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Application.Constants;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.Categories.GetCategoryOptionValuesByIdQuery
{
    public class GetCategoryOptionValuesByIdQueryHandler : IRequestHandler<GetCategoryOptionValuesByIdQuery,
        IDataResult<CategoryOptionValueDto>>
    {
        private readonly ICategoryOptionValueRepository _categoryOptionValueRepository;
        private readonly IMapper _mapper;

        public GetCategoryOptionValuesByIdQueryHandler(ICategoryOptionValueRepository categoryOptionValueRepository, IMapper mapper)
        {
            _categoryOptionValueRepository = categoryOptionValueRepository;
            _mapper = mapper;
        }

        public async Task<IDataResult<CategoryOptionValueDto>> Handle(GetCategoryOptionValuesByIdQuery request,
            CancellationToken cancellationToken)
        {
            var categoryOptionValue =
                await _categoryOptionValueRepository.GetAsync(x => x.Category.Id.Equals(request.Id));
            if (categoryOptionValue is null)
            {
                return new ErrorDataResult<CategoryOptionValueDto>(Messages.DataNotFound);
            }

            var result = _mapper.Map<CategoryOptionValueDto>(categoryOptionValue);
            return new SuccessDataResult<CategoryOptionValueDto>(result);
        }
    }
}