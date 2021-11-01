using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Application.Constants;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.Aspects.Autofac.Validation;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Brands.UpdateBrandCommand
{
    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, IDataResult<BrandDto>>
    {
        private readonly IMapper _mapper;
        private readonly IBrandRepository _brandRepository;

        public UpdateBrandCommandHandler(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        [ValidationAspect(typeof(UpdateBrandCommandValidator))]
        public async Task<IDataResult<BrandDto>> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var isAlreadyExist = await _brandRepository.GetAsync(x => x.NormalizedName.Equals(request.Name.ToLower()));
            if (isAlreadyExist is not null)
            {
                return new ErrorDataResult<BrandDto>(Messages.DataAlreadyExist);
            }

            var brand = await _brandRepository.GetByIdAsync(request.Id);
            if (brand is null)
            {
                return new ErrorDataResult<BrandDto>(Messages.DataNotFound);
            }

            var updateBrand = _mapper.Map(request, brand);
            updateBrand.LastUpdatedBy = "admin";
            updateBrand.LastUpdatedDate = DateTime.Now;
            updateBrand.NormalizedName = updateBrand.Name.ToLower();
            await _brandRepository.UpdateAsync(brand.Id, updateBrand);

            var brandDto = _mapper.Map<BrandDto>(brand);
            return new SuccessDataResult<BrandDto>(brandDto);
        }
    }
}