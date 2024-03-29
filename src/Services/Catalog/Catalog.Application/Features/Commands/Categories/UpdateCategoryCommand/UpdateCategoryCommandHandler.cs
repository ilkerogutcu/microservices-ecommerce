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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateCategoryCommandHandler(IMapper mapper, ICategoryRepository categoryRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        [ValidationAspect(typeof(UpdateCategoryCommandValidator))]
        public async Task<IDataResult<Category>> Handle(UpdateCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var currentUserId = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)
                ?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return new ErrorDataResult<Category>(Messages.SignInFirst);
            }

            var mainCategory = await _categoryRepository.GetByIdAsync(request.MainCategoryId);
            if (mainCategory is null) return new ErrorDataResult<Category>(Messages.DataNotFound);

            var mainCategoryAlreadyExists =
                await _categoryRepository.AnyAsync(x => x.Name.Equals(request.Name) && !x.Id.Equals(mainCategory.Id));
            if (mainCategoryAlreadyExists)
            {
                return new ErrorDataResult<Category>(Messages.DataAlreadyExist);
            }

            if (string.IsNullOrEmpty(request.SubCategoryId))
            {
                mainCategory = _mapper.Map(request, mainCategory).Update(request.Name, request.IsActive, currentUserId);
                await _categoryRepository.UpdateAsync(mainCategory.Id, mainCategory);
                return new SuccessDataResult<Category>(mainCategory);
            }

            mainCategory.SubCategories
                .Map(p => p.Id.Equals(request.SubCategoryId), n => n.SubCategories)
                .FirstOrDefault()
                ?.Update(request.Name, request.IsActive, currentUserId);
            var result = await _categoryRepository.UpdateAsync(mainCategory.Id, mainCategory);
            return new SuccessDataResult<Category>(result);
        }
    }
}