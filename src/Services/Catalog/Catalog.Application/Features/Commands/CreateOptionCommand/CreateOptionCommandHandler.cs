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

namespace Catalog.Application.Features.Commands.CreateOptionCommand
{
    public class CreateOptionCommandHandler : IRequestHandler<CreateOptionCommand, IDataResult<Option>>
    {
        private readonly IMapper _mapper;
        private readonly IOptionRepository _optionRepository;

        public CreateOptionCommandHandler(IMapper mapper, IOptionRepository optionRepository)
        {
            _mapper = mapper;
            _optionRepository = optionRepository;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        [ValidationAspect(typeof(CreateOptionCommandValidator))]
        public async Task<IDataResult<Option>> Handle(CreateOptionCommand request, CancellationToken cancellationToken)
        {
            var isAlreadyExists =
                await _optionRepository.GetAsync(x => x.NormalizedName.Equals(request.Name.ToLower()));

            if (isAlreadyExists is not null)
            {
                return new SuccessDataResult<Option>(Messages.DataAlreadyExist);
            }

            var option = _mapper.Map<Option>(request);
            option.CreatedBy = "admin";
            option.CreatedDate = DateTime.Now;
            option.NormalizedName = request.Name.ToLower();

            await _optionRepository.AddAsync(option);
            return new SuccessDataResult<Option>(option);
        }
    }
}