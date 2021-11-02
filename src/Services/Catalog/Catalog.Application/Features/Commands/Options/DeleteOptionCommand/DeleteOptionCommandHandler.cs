﻿using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Constants;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.Aspects.Autofac.Validation;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Options.DeleteOptionCommand
{
    public class DeleteOptionCommandHandler: IRequestHandler<DeleteOptionCommand, IResult>
    {
        private readonly IOptionRepository _optionRepository;

        public DeleteOptionCommandHandler(IOptionRepository optionRepository)
        {
            _optionRepository = optionRepository;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        [ValidationAspect(typeof(DeleteOptionCommandValidator))]
        public async Task<IResult> Handle(DeleteOptionCommand request, CancellationToken cancellationToken)
        {
            var option = await _optionRepository.GetByIdAsync(request.Id);
            if (option is null)
            {
                return new ErrorResult(Messages.DataNotFound);
            }

            await _optionRepository.DeleteAsync(option);
            return new SuccessResult();
        }
    }
}