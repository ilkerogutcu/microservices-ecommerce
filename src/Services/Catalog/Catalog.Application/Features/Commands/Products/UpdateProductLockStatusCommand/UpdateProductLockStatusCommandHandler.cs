using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Constants;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.Aspects.Autofac.Validation;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Products.UpdateProductLockStatusCommand
{
    public class UpdateProductLockStatusCommandHandler : IRequestHandler<UpdateProductLockStatusCommand, IResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateProductLockStatusCommandHandler(IProductRepository productRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        [ValidationAspect(typeof(UpdateProductLockStatusCommandValidator))]
        public async Task<IResult> Handle(UpdateProductLockStatusCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)
                ?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return new ErrorResult(Messages.SignInFirst);
            }

            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product is null) return new ErrorResult(Messages.DataNotFound);

            product.Locked = request.LockStatus;
            product.LastUpdatedBy = currentUserId;
            product.LastUpdatedDate = DateTime.Now;
            await _productRepository.UpdateAsync(product.Id, product);
            return new SuccessResult();
        }
    }
}