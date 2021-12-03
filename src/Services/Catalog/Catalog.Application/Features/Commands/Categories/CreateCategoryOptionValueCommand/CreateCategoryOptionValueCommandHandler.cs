﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Constants;
using Catalog.Application.Extensions;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using MediatR;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.Aspects.Autofac.Validation;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Categories.CreateCategoryOptionValueCommand
{
    public class CreateOrUpdateCategoryOptionValueCommandHandler : IRequestHandler<
        CreateCategoryOptionValueCommand,
        IDataResult<CategoryOptionValue>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IOptionValueRepository _optionValueRepository;
        private readonly IOptionRepository _optionRepository;
        private readonly ICategoryOptionValueRepository _categoryOptionValueRepository;
        
        public CreateOrUpdateCategoryOptionValueCommandHandler(ICategoryRepository categoryRepository,
            IOptionValueRepository optionValueRepository, ICategoryOptionValueRepository categoryOptionValueRepository,
            IOptionRepository optionRepository)
        {
            _categoryRepository = categoryRepository;
            _optionValueRepository = optionValueRepository;
            _categoryOptionValueRepository = categoryOptionValueRepository;
            _optionRepository = optionRepository;
        }
        
        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        [ValidationAspect(typeof(CreateCategoryOptionValueCommandValidator))]
        public async Task<IDataResult<CategoryOptionValue>> Handle(CreateCategoryOptionValueCommand request,
            CancellationToken cancellationToken)
        {
          
            var category = (await _categoryRepository.GetListAsync()).AsEnumerable()
                .Map(p => p.Id.Equals(request.CategoryId), n => n.SubCategories)
                .FirstOrDefault();
            var option = await _optionRepository.GetByIdAsync(request.OptionId);
            if (category == null || option == null)
            {
                return new ErrorDataResult<CategoryOptionValue>(Messages.DataNotFound);
            }

            var categoryOptionValue = new CategoryOptionValue();
            foreach (var optionValueId in request.OptionValueIds)
            {
                var optionValue = await _optionValueRepository.GetByIdAsync(optionValueId);
                if (optionValue is not null)
                {
                    categoryOptionValue.OptionValues.Add(optionValue);
                }
            }

            if (categoryOptionValue.OptionValues.Count <= 0)
                return new ErrorDataResult<CategoryOptionValue>(Messages.DataNotFound);

            categoryOptionValue.Option = option;
            categoryOptionValue.LastUpdatedBy = "admin";
            categoryOptionValue.LastUpdatedDate = DateTime.Now;
            categoryOptionValue.Varianter = request.Varianter;
            categoryOptionValue.IsRequired = request.IsRequired;
            await _categoryOptionValueRepository.UpdateAsync(categoryOptionValue.Id, categoryOptionValue);
            return new SuccessDataResult<CategoryOptionValue>(categoryOptionValue);
        }
    }
}