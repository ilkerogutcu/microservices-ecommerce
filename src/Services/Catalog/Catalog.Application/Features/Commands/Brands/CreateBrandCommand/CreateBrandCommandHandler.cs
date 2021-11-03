using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Application.Constants;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using MediatR;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.Aspects.Autofac.Validation;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Brands.CreateBrandCommand
{
    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, IDataResult<Brand>>
    {
        private readonly IMapper _mapper;
        private readonly IBrandRepository _brandRepository;

        public CreateBrandCommandHandler(IMapper mapper, IBrandRepository brandRepository)
        {
            _mapper = mapper;
            _brandRepository = brandRepository;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        [ValidationAspect(typeof(CreateBrandCommandValidator))]
        public async Task<IDataResult<Brand>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var isAlreadyExist = await _brandRepository.AnyAsync(x => x.NormalizedName.Equals(request.Name.ToLower()));
            if (isAlreadyExist)
            {
                return new ErrorDataResult<Brand>(Messages.DataAlreadyExist);
            }

            var brand = _mapper.Map<Brand>(request);
            brand.CreatedBy = "admin";
            brand.CreatedDate = DateTime.Now;
            brand.NormalizedName = request.Name.ToLower();

            await _brandRepository.AddAsync(brand);
            return new SuccessDataResult<Brand>(brand);
        }
    }
}