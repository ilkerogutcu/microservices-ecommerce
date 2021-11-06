using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.Categories.GetAllCategoriesQuery
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IDataResult<List<CategoryDto>>>
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        
        public async Task<IDataResult<List<CategoryDto>>> Handle(GetAllCategoriesQuery request,
            CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetListAsync();

            if (request.IsActive is not null)
            {
                categories = categories.Where(x => x.IsActive == request.IsActive);
            }

            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories).OrderBy(x => x.Name).ToList();
            return new SuccessDataResult<List<CategoryDto>>(categoriesDto);
        }
    }
}