using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateProductCommandHandler(IProductRepository productRepository, IMapper mapper,
            ICategoryRepository categoryRepository, IBrandRepository brandRepository,
            ICategoryOptionValueRepository categoryOptionValueRepository, IOptionValueRepository optionValueRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _categoryOptionValueRepository = categoryOptionValueRepository;
            _optionValueRepository = optionValueRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        [ValidationAspect(typeof(CreateManyProductsCommandValidator))]
        public async Task<IDataResult<List<Product>>> Handle(CreateManyProductsCommand request,
            CancellationToken cancellationToken)
        {
            var currentUserId = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)
                ?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return new ErrorDataResult<List<Product>>(Messages.SignInFirst);
            }

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

                product.NormalizedName = product.Name.ToLower();
                product.IsFreeShipping = product.SalePrice > 100;
                product.ThumbnailImageUrl = product.ImageUrls[0];
                product.Brand = brand;
                product.Category = category;
                product.CreatedDate = DateTime.Now;
                product.CreatedBy = currentUserId;
                await _productRepository.AddAsync(product);
                productList.Add(product);
                messages.Add(
                    $"Product Name: {product.Name} Model Code: {product.ModelCode} Barcode: {product.Barcode} added successfully.");
            }

            return new SuccessDataResult<List<Product>>(productList, string.Join("///", messages));
        }

        private bool CheckRequiredOptions(CreateProductDto createProductDto, Category category,
            ref List<string> messages)
        {
            var messagesCountTemp = messages.Count;
            var remainingRequiredOptionValues = _categoryOptionValueRepository.GetList(
                x => x.Category.Id.Equals(category.Id) && x.IsRequired).ToList();

            if (remainingRequiredOptionValues.Count is 0)
            {
                return true;
            }

            foreach (var optionValueId in createProductDto.OptionValueIds)
            {
                var requiredCategoryOptionValue = remainingRequiredOptionValues.FirstOrDefault(
                    x => x.OptionValues.Any(x => x.Id.Equals(optionValueId)));

                if (requiredCategoryOptionValue is not null)
                {
                    remainingRequiredOptionValues.Remove(requiredCategoryOptionValue);
                }
            }

            messages.AddRange(remainingRequiredOptionValues.Select(remainingRequiredOptionValue =>
                $"{remainingRequiredOptionValue.Option.Name} is required!"));

            return messagesCountTemp == messages.Count;
        }
    }
}