using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Application.Constants;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using MediatR;
using MongoDB.Bson;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.Aspects.Autofac.Validation;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Categories.CreateCategoryCommand
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, IDataResult<Category>>
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryCommandHandler(IMapper mapper, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        [ValidationAspect(typeof(CreateCategoryCommandValidator))]
        public async Task<IDataResult<Category>> Handle(CreateCategoryCommand request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.SubCategoryId) && string.IsNullOrEmpty(request.MainCategoryId))
            {
                var isAlreadyExist = await _categoryRepository.AnyAsync(x => x.Name.Equals(request.Name));
                if (isAlreadyExist)
                {
                    return new ErrorDataResult<Category>(Messages.DataAlreadyExist);
                }

                var category = _mapper.Map<Category>(request);
                category.CreatedBy = "admin";
                category.CreatedDate = DateTime.Now;
                await _categoryRepository.AddAsync(category);
                return new SuccessDataResult<Category>(category);
            }

            //Todo refactor
            var mainCategory = await _categoryRepository.GetByIdAsync(request.MainCategoryId);
            if (mainCategory is null) return new ErrorDataResult<Category>(Messages.DataNotFound);
            if (string.IsNullOrEmpty(request.SubCategoryId))
            {
                mainCategory.AddSubCategory(new Category
                {
                    Name = request.Name,
                    IsActive = request.IsActive,
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now
                });
                return new SuccessDataResult<Category>(
                    await _categoryRepository.UpdateAsync(request.MainCategoryId, mainCategory));
            }

            if (!string.IsNullOrEmpty(request.SubCategoryId) && !string.IsNullOrEmpty(request.MainCategoryId))
            {
                mainCategory.GetCategoryByIdFromSubCategories(mainCategory.SubCategories,
                    request.SubCategoryId).AddSubCategory(new Category
                {
                    Name = request.Name,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "admin"
                });

                return new SuccessDataResult<Category>(
                    await _categoryRepository.UpdateAsync(request.MainCategoryId, mainCategory));
            }

            return new ErrorDataResult<Category>(Messages.InvalidParameter);

        }
    }
}