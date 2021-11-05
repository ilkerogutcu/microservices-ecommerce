using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Application.Constants;
using Catalog.Application.Extensions;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using MediatR;
using MongoDB.Bson;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.Aspects.Autofac.Validation;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Categories.UpdateCategoryCommand
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, IDataResult<Category>>
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryCommandHandler(IMapper mapper, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        [ValidationAspect(typeof(UpdateCategoryCommandValidator))]
        public async Task<IDataResult<Category>> Handle(UpdateCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var mainCategory = await _categoryRepository.GetByIdAsync(request.MainCategoryId);
            if (mainCategory is null) return new ErrorDataResult<Category>(Messages.DataNotFound);

            if (string.IsNullOrEmpty(request.SubCategoryId))
            {
                mainCategory = _mapper.Map(request, mainCategory);
                mainCategory.LastUpdatedBy = "admin";
                mainCategory.LastUpdatedDate = DateTime.Now;
                await _categoryRepository.UpdateAsync(mainCategory.Id, mainCategory);
                return new SuccessDataResult<Category>(mainCategory);
            }


            mainCategory.SubCategories
                .Map(p => p.Id.Equals(request.SubCategoryId), n => n.SubCategories)
                .FirstOrDefault()
                ?.Update(request.Name, request.IsActive,"admin");
            var result = await _categoryRepository.UpdateAsync(mainCategory.Id, mainCategory);
            return new SuccessDataResult<Category>(result);
        }
    }
}