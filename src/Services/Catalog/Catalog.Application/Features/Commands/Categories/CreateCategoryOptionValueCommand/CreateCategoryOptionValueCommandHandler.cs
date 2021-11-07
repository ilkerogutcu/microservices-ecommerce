using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Application.Constants;
using Catalog.Application.Extensions;
using Catalog.Application.Features.Commands.Categories.CreateCategoryCommand;
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
    public class CreateCategoryOptionValueCommandHandler : IRequestHandler<CreateCategoryOptionValueCommand,
        IDataResult<CategoryOptionValue>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IOptionValueRepository _optionValueRepository;
        private readonly ICategoryOptionValueRepository _categoryOptionValueRepository;


        public CreateCategoryOptionValueCommandHandler(ICategoryRepository categoryRepository,
            IOptionValueRepository optionValueRepository, ICategoryOptionValueRepository categoryOptionValueRepository)
        {
            _categoryRepository = categoryRepository;
            _optionValueRepository = optionValueRepository;
            _categoryOptionValueRepository = categoryOptionValueRepository;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        [ValidationAspect(typeof(CreateCategoryOptionValueCommandValidator))]
        public async Task<IDataResult<CategoryOptionValue>> Handle(CreateCategoryOptionValueCommand request,
            CancellationToken cancellationToken)
        {
            var isAlreadyExist = await _categoryOptionValueRepository.AnyAsync(x =>
                x.Category.Id.Equals(request.CategoryId));
            if (isAlreadyExist)
            {
                return new ErrorDataResult<CategoryOptionValue>(Messages.DataAlreadyExist);
            }

            var category = (await _categoryRepository.GetListAsync()).AsEnumerable()
                .Map(p => p.Id.Equals(request.CategoryId), n => n.SubCategories)
                .FirstOrDefault();


            var categoryOptionValues = new CategoryOptionValue();

            foreach (var optionValueId in request.OptionValueIds)
            {
                var optionValue = await _optionValueRepository.GetByIdAsync(optionValueId);
                if (optionValue is not null)
                {
                    categoryOptionValues.OptionValues.Add(optionValue);
                }
            }

            if (category == null || categoryOptionValues.OptionValues.Count <= 0)
                return new ErrorDataResult<CategoryOptionValue>(Messages.DataNotFound);

            categoryOptionValues.Category = category;
            categoryOptionValues.CreatedDate = DateTime.Now;
            categoryOptionValues.CreatedBy = "admin";
            await _categoryOptionValueRepository.AddAsync(categoryOptionValues);

            return new SuccessDataResult<CategoryOptionValue>(categoryOptionValues);
        }
    }
}