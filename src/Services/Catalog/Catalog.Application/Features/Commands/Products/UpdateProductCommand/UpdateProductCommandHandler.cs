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
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.Aspects.Autofac.Validation;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Products.UpdateProductCommand
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, IDataResult<Product>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ICategoryOptionValueRepository _categoryOptionValueRepository;
        private readonly IOptionValueRepository _optionValueRepository;

        public UpdateProductCommandHandler(ICategoryRepository categoryRepository, IBrandRepository brandRepository,
            IProductRepository productRepository, IMapper mapper,
            ICategoryOptionValueRepository categoryOptionValueRepository, IOptionValueRepository optionValueRepository)
        {
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _categoryOptionValueRepository = categoryOptionValueRepository;
            _optionValueRepository = optionValueRepository;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        [ValidationAspect(typeof(UpdateProductCommandValidator))]
        public async Task<IDataResult<Product>> Handle(UpdateProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product is null)
            {
                return new ErrorDataResult<Product>(Messages.DataNotFound);
            }

            var brand = await _brandRepository.GetByIdAsync(request.BrandId);
            var category = (await _categoryRepository.GetListAsync()).AsEnumerable()
                .Map(p => p.Id.Equals(request.CategoryId), n => n.SubCategories)
                .FirstOrDefault();
            if (brand is null || category is null)
            {
                return new ErrorDataResult<Product>(Messages.DataNotFound);
            }

            product = _mapper.Map(request, product);

            product.OptionValues.Clear();
            foreach (var optionValueId in request.OptionValueIds)
            {
                var optionValue = await _optionValueRepository.GetByIdAsync(optionValueId);
                if (optionValue is null)
                {
                    return new ErrorDataResult<Product>(Messages.DataNotFound);
                }

                product.AddOptionValue(optionValue);
            }

            product.ThumbnailImageUrl = product.ImageUrls[0];
            product.LastUpdatedBy = "admin";
            product.LastUpdatedDate = DateTime.Now;
            await _productRepository.UpdateAsync(product.Id, product);
            return new SuccessDataResult<Product>(product);
        }
    }
}