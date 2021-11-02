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

namespace Catalog.Application.Features.Commands.OptionValues.CreateOptionValueCommand
{
    public class CreateOptionValueCommandHandler : IRequestHandler<CreateOptionValueCommand, IDataResult<OptionValue>>
    {
        private readonly IMapper _mapper;
        private readonly IOptionValueRepository _optionValueRepository;
        private readonly IOptionRepository _optionRepository;

        public CreateOptionValueCommandHandler(IMapper mapper, IOptionValueRepository optionValueRepository,
            IOptionRepository optionRepository)
        {
            _mapper = mapper;
            _optionValueRepository = optionValueRepository;
            _optionRepository = optionRepository;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        [ValidationAspect(typeof(CreateOptionValueCommandValidator))]
        public async Task<IDataResult<OptionValue>> Handle(CreateOptionValueCommand request,
            CancellationToken cancellationToken)
        {
            var option = await _optionRepository.GetByIdAsync(request.OptionId);
            if (option is null)
            {
                return new ErrorDataResult<OptionValue>(Messages.DataNotFound);
            }

            var isAlreadyExists = await _optionValueRepository.AnyAsync(x =>
                x.OptionId.Equals(option.Id) && x.NormalizedName.Equals(request.Name.ToLower()));
            if (isAlreadyExists)
            {
                return new ErrorDataResult<OptionValue>(Messages.DataAlreadyExist);
            }

            var optionValue = _mapper.Map<OptionValue>(request);
            optionValue.NormalizedName = request.Name.ToLower();
            optionValue.CreatedBy = "admin";
            optionValue.CreatedDate = DateTime.Now;

            await _optionValueRepository.AddAsync(optionValue);
            return new SuccessDataResult<OptionValue>(optionValue);
        }
    }
}