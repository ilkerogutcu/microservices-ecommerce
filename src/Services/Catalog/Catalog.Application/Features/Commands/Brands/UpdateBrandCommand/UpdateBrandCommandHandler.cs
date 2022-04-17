using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Application.Constants;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using MediatR;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.Aspects.Autofac.Validation;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Brands.UpdateBrandCommand
{
    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, IDataResult<Brand>>
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
        public async Task<IDataResult<Brand>> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await _brandRepository.GetByIdAsync(request.Id);
            if (brand is null)
            {
                return new ErrorDataResult<Brand>(Messages.DataNotFound);
            }

            var isAlreadyExist =  await _brandRepository.AnyAsync(x =>
                x.NormalizedName.Equals(request.Name.ToLower()) && !x.NormalizedName.Equals(brand.NormalizedName));
            if (isAlreadyExist)
            {
                return new ErrorDataResult<Brand>(Messages.DataAlreadyExist);
            }

            brand = _mapper.Map(request, brand);
            brand.LastUpdatedBy = "admin";
            brand.LastUpdatedDate = DateTime.Now;
            brand.NormalizedName = brand.Name.ToLower();
            await _brandRepository.UpdateAsync(brand.Id, brand);

            return new SuccessDataResult<Brand>(brand);
        }
    }
}