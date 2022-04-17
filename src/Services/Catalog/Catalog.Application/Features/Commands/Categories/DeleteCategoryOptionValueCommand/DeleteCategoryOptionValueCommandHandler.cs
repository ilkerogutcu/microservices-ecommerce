using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.Aspects.Autofac.Validation;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Categories.DeleteCategoryOptionValueCommand
{
    public class DeleteCategoryOptionValueCommandHandler: IRequestHandler<DeleteCategoryOptionValueCommand, IResult>
    {
        private readonly ICategoryOptionValueRepository _categoryOptionValueRepository;

        public DeleteCategoryOptionValueCommandHandler(ICategoryOptionValueRepository categoryOptionValueRepository)
        {
            _categoryOptionValueRepository = categoryOptionValueRepository;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        [ValidationAspect(typeof(DeleteCategoryOptionValueCommandValidator))]
        public async Task<IResult> Handle(DeleteCategoryOptionValueCommand request, CancellationToken cancellationToken)
        {
            await _categoryOptionValueRepository.DeleteByIdAsync(request.Id);
            return new SuccessResult();
        }
    }
}