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

namespace Catalog.Application.Features.Commands.Options.UpdateOptionCommand
{
    public class UpdateOptionCommandHandler : IRequestHandler<UpdateOptionCommand, IDataResult<Option>>
    {
        private readonly IMapper _mapper;
        private readonly IOptionRepository _optionRepository;

        public UpdateOptionCommandHandler(IMapper mapper, IOptionRepository optionRepository)
        {
            _mapper = mapper;
            _optionRepository = optionRepository;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        [ValidationAspect(typeof(UpdateOptionCommandValidator))]
        public async Task<IDataResult<Option>> Handle(UpdateOptionCommand request, CancellationToken cancellationToken)
        {
            var option = await _optionRepository.GetByIdAsync(request.Id);
            if (option is null)
            {
                return new ErrorDataResult<Option>(Messages.DataNotFound);
            }

            var isAlreadyExist = await _optionRepository.GetAsync(x => x.NormalizedName.Equals(request.Name.ToLower()));
            if (isAlreadyExist is not null && !isAlreadyExist.Name.Equals(option.Name))
            {
                return new ErrorDataResult<Option>(Messages.DataAlreadyExist);
            }

            option = _mapper.Map(request, option);
            option.LastUpdatedBy = "admin";
            option.LastUpdatedDate = DateTime.Now;
            option.NormalizedName = request.Name.ToLower();
            await _optionRepository.UpdateAsync(option.Id, option);

            return new SuccessDataResult<Option>(option);
        }
    }
}