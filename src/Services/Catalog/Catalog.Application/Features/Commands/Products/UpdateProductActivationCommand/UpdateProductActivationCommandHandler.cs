using System;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Constants;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.Aspects.Autofac.Validation;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Products.UpdateProductActivationCommand
{
    public class UpdateProductActivationCommandHandler : IRequestHandler<UpdateProductActivationCommand, IResult>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductActivationCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        [ValidationAspect(typeof(UpdateProductActivationCommandValidator))]
        public async Task<IResult> Handle(UpdateProductActivationCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product is null)
            {
                return new ErrorResult(Messages.DataNotFound);
            }

            product.IsActive = request.IsActive;
            product.LastUpdatedBy = "admin";
            product.LastUpdatedDate = DateTime.Now;
            await _productRepository.UpdateAsync(product.Id, product);
            return new SuccessResult();
        }
    }
}