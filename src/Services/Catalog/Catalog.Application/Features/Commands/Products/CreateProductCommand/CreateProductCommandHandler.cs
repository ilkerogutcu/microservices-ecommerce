using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Application.Constants;
using Catalog.Application.Extensions;
using Catalog.Application.Interfaces;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using MediatR;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Products.CreateProductCommand
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, IDataResult<Product>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IMediaGrpcService _mediaGrpcService;

        public CreateProductCommandHandler(IProductRepository productRepository, IMapper mapper,
            ICategoryRepository categoryRepository, IBrandRepository brandRepository,
            IMediaGrpcService mediaGrpcService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _mediaGrpcService = mediaGrpcService;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        public async Task<IDataResult<Product>> Handle(CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            var brand = await _brandRepository.GetByIdAsync(request.BrandId);
            var category = (await _categoryRepository.GetListAsync()).AsEnumerable()
                .Map(p => p.Id.Equals(request.CategoryId), n => n.SubCategories)
                .FirstOrDefault();

            if (brand is null)
            {
                return new ErrorDataResult<Product>(Messages.BrandNotFound);
            }

            if (category is null)
            {
                return new ErrorDataResult<Product>(Messages.CategoryNotFound);
            }
            
            
            var product = _mapper.Map<Product>(request);
            product.MediaList = await _mediaGrpcService.UploadImage(request.FileList);
            if (product.MediaList.Count<=0)
            {
                return new ErrorDataResult<Product>(Messages.ErrorWhileUploadingMedia);
            }
            product.ThumbnailMedia = product.MediaList[0];
            product.Brand = brand;
            product.Category = category;
            product.CreatedDate = DateTime.Now;
            product.CreatedBy = "admin";
            await _productRepository.AddAsync(product);
            return new SuccessDataResult<Product>(product);
        }
    }
}