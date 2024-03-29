﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Application.Constants;
using Catalog.Application.Extensions;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateCategoryCommandHandler(IMapper mapper, ICategoryRepository categoryRepository, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        [ValidationAspect(typeof(CreateCategoryCommandValidator))]
        public async Task<IDataResult<Category>> Handle(CreateCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var currentUserId = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)
                ?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return new ErrorDataResult<Category>(Messages.SignInFirst);
            }
            var isAlreadyExists = (await _categoryRepository.GetListAsync()).AsEnumerable().Map(p => p.Name.Equals(request.Name), n => n.SubCategories)
                .Any();
            
            if (isAlreadyExists)
            {
                return new ErrorDataResult<Category>(Messages.DataAlreadyExist);
            }

            if (string.IsNullOrEmpty(request.SubCategoryId) && string.IsNullOrEmpty(request.MainCategoryId))
            {
                var category = _mapper.Map<Category>(request);
                category.CreatedBy = "admin";
                category.CreatedDate = DateTime.Now;
                await _categoryRepository.AddAsync(category);
                return new SuccessDataResult<Category>(category);
            }

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

            mainCategory.SubCategories
                .Map(p => p.Id.Equals(request.SubCategoryId), n => n.SubCategories)
                .FirstOrDefault()
                ?.AddSubCategory(new Category
                {
                    Name = request.Name,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "admin"
                });
            return new SuccessDataResult<Category>(
                await _categoryRepository.UpdateAsync(request.MainCategoryId, mainCategory));
        }
    }
}