using System;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Constants;
using Catalog.Application.Features.Events.Products;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using MediatR;
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

        public AddCommentToProductCommandHandler(IProductRepository productRepository, IMediator mediator)
        {
            _productRepository = productRepository;
            _mediator = mediator;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        [ValidationAspect(typeof(AddCommentToProductCommandValidator))]
        public async Task<IResult> Handle(AddCommentToProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product is null)
            {
                return new ErrorResult(Messages.DataNotFound);
            }

            product.AddComment(new Comment
            {
                Content = request.CommentContent,
                Rating = request.ProductRating,
                CreatedBy = "admin",
            });
            await _productRepository.UpdateAsync(product.Id, product);

             _mediator.Publish(new CommentAddedToProductEvent(product.Id, request.ProductRating));
            return new SuccessResult();
        }
    }
}