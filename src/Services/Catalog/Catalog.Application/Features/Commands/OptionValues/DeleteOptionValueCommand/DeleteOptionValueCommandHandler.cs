using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Application.Constants;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.Aspects.Autofac.Validation;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.OptionValues.DeleteOptionValueCommand
{
    public class DeleteOptionValueCommandHandler : IRequestHandler<DeleteOptionValueCommand, IResult>
    {
        private readonly IOptionValueRepository _optionValueRepository;

        public DeleteOptionValueCommandHandler(IOptionValueRepository optionValueRepository)
        {
            _optionValueRepository = optionValueRepository;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        [ValidationAspect(typeof(DeleteOptionValueCommandValidator))]
        public async Task<IResult> Handle(DeleteOptionValueCommand request, CancellationToken cancellationToken)
        {
            var optionValue = await _optionValueRepository.GetByIdAsync(request.Id);
            if (optionValue is null)
            {
                return new ErrorResult(Messages.DataNotFound);
            }

            await _optionValueRepository.DeleteAsync(optionValue);
            return new SuccessResult();
        }
    }
}