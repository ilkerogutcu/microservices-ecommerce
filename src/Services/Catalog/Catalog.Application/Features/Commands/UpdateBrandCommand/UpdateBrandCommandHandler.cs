using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.Aspects.Autofac.Validation;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.UpdateBrandCommand
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
            var brand = await _brandRepository.GetByIdAsync(request.BrandDto.Id);
            var updateBrand = _mapper.Map(request.BrandDto, brand);
            updateBrand.LastUpdatedBy = "admin";
            updateBrand.LastUpdatedDate = DateTime.Now;
            await _brandRepository.UpdateAsync(brand.Id, updateBrand);
            return new SuccessDataResult<BrandDto>(request.BrandDto);
        }
    }
}