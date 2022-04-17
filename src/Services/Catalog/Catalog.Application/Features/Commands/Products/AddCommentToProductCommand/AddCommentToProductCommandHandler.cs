using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Constants;
using Catalog.Application.Features.Events.Products;
using Catalog.Application.Features.Events.Products.CommentAddedToProductEvent;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.Aspects.Autofac.Validation;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Products.AddCommentToProductCommand
{
    public class AddCommentToProductCommandHandler : IRequestHandler<AddCommentToProductCommand, IResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddCommentToProductCommandHandler(IProductRepository productRepository, IMediator mediator,
            IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        [ValidationAspect(typeof(AddCommentToProductCommandValidator))]
        public async Task<IResult> Handle(AddCommentToProductCommand request, CancellationToken cancellationToken)

        {
            var currentUserId = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)
                ?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return new ErrorResult(Messages.SignInFirst);
            }

            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product is null)
            {
                return new ErrorResult(Messages.DataNotFound);
            }

            product.AddComment(new Comment
            {
                Content = request.CommentContent,
                Rating = request.ProductRating,
                CreatedBy = currentUserId,
            });
            await _productRepository.UpdateAsync(product.Id, product);

            _mediator.Publish(new CommentAddedToProductEvent(product.Id, request.ProductRating));
            return new SuccessResult();
        }
    }
}