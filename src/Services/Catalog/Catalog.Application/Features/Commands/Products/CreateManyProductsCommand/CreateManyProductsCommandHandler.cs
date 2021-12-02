using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Application.Constants;
using Catalog.Application.Dtos;
using Catalog.Application.Extensions;
using Catalog.Application.Interfaces;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using MediatR;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.Aspects.Autofac.Validation;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Products.CreateManyProductsCommand
{
    public class CreateProductCommandHandler : IRequestHandler<CreateManyProductsCommand, IDataResult<List<Product>>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ICategoryOptionValueRepository _categoryOptionValueRepository;
        private readonly IOptionValueRepository _optionValueRepository;

        public CreateProductCommandHandler(IProductRepository productRepository, IMapper mapper,
            ICategoryRepository categoryRepository, IBrandRepository brandRepository,
            ICategoryOptionValueRepository categoryOptionValueRepository, IOptionValueRepository optionValueRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _categoryOptionValueRepository = categoryOptionValueRepository;
            _optionValueRepository = optionValueRepository;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        [ValidationAspect(typeof(CreateManyProductsCommandValidator))]
        public async Task<IDataResult<List<Product>>> Handle(CreateManyProductsCommand request,
            CancellationToken cancellationToken)
        {
            var messages = new List<string>();
            var productList = new List<Product>();
            foreach (var createProductDto in request.Products)
            {
                var brand = await _brandRepository.GetByIdAsync(createProductDto.BrandId);
                var category = (await _categoryRepository.GetListAsync()).AsEnumerable()
                    .Map(p => p.Id.Equals(createProductDto.CategoryId), n => n.SubCategories)
                    .FirstOrDefault();

                if (brand is null)

                {
                    messages.Add($"Brand not found for {createProductDto.Name}!");
                    continue;
                }

                if (category is null)
                {
                    messages.Add($"Category not found for {createProductDto.Name}!");
                    continue;
                }

                if (!CheckRequiredOptions(createProductDto, category, ref messages))
                {
                    continue;
                }

                var product = _mapper.Map<Product>(createProductDto);

                foreach (var optionValueId in createProductDto.OptionValueIds)
                {
                    var optionValue = await _optionValueRepository.GetByIdAsync(optionValueId);
                    if (optionValue is null)
                    {
                        messages.Add($"Data not found for {optionValueId}");
                        continue;
                    }

                    product.AddOptionValue(optionValue);
                }

                if (product.OptionValues.Count <= 0)
                {
                    messages.Add($"{product.Name} cannot be added because the option value is not found.");
                    continue;
                }

                product.IsFreeShipping = product.SalePrice > 100;
                product.ThumbnailImageUrl = product.ImageUrls[0];
                product.Brand = brand;
                product.Category = category;
                product.CreatedDate = DateTime.Now;
                product.CreatedBy = "admin";
                await _productRepository.AddAsync(product);
                productList.Add(product);
                messages.Add($"Product Name:{product.Name} Model Code:{product.ModelCode} Barcode:{product.Barcode} added successfully.");
            }

            return new SuccessDataResult<List<Product>>(productList, string.Join("///", messages));
        }

        private bool CheckRequiredOptions(CreateProductDto createProductDto, Category category,
            ref List<string> messages)
        {
            var messagesCountTemp = messages.Count;
            if (_categoryOptionValueRepository.Any(x => x.Category.Id.Equals(category.Id) && x.IsRequired))
            {
                var categoryOptionValues =
                    _categoryOptionValueRepository.GetList(x => x.Category.Id.Equals(category.Id));

                foreach (var categoryOptionValue in categoryOptionValues)
                {
                    messages.AddRange(from optionValueId in createProductDto.OptionValueIds
                        where categoryOptionValue.IsRequired &&
                              !categoryOptionValue.OptionValues.Any(x => x.Id.Equals(optionValueId))
                        select $"{categoryOptionValue.Option.Name} is required!");
                }
            }

            return messagesCountTemp == messages.Count;
        }
    }
}