using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Application.Constants;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.Categories.GetCategoryOptionValuesByIdQuery
{
    public class GetCategoryOptionValuesByIdQueryHandler : IRequestHandler<GetCategoryOptionValuesByIdQuery,
        IDataResult<GetCategoryOptionValuesByIdViewModel>>
    {
        private readonly ICategoryOptionValueRepository _categoryOptionValueRepository;
        private readonly IMapper _mapper;

        public GetCategoryOptionValuesByIdQueryHandler(ICategoryOptionValueRepository categoryOptionValueRepository,
            IMapper mapper)
        {
            _categoryOptionValueRepository = categoryOptionValueRepository;
            _mapper = mapper;
        }

        public async Task<IDataResult<GetCategoryOptionValuesByIdViewModel>> Handle(
            GetCategoryOptionValuesByIdQuery request,
            CancellationToken cancellationToken)
        {
            var categoryOptionValues =
                await _categoryOptionValueRepository.GetListAsync(x => x.Category.Id.Equals(request.CategoryId));
            if (!categoryOptionValues.Any())
            {
                return new ErrorDataResult<GetCategoryOptionValuesByIdViewModel>(Messages.DataNotFound);
            }

            var category = categoryOptionValues.First().Category;

            var result = new GetCategoryOptionValuesByIdViewModel()
            {
                CategoryId = category.Id,
                CategoryName = category.Name,
                CategoryOptionValues = _mapper.Map<List<CategoryOptionValueDto>>(categoryOptionValues)
            };

            return new SuccessDataResult<GetCategoryOptionValuesByIdViewModel>(result);
        }
    }
}