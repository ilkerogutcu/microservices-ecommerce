using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Constants;
using Catalog.Application.Features.Events.Products.ProductViewedEvent;
using Catalog.Application.Features.Queries.Catalog.ViewModels;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.Catalog.GetProductDetailsByIdQuery
{
    public class GetProductDetailsByIdQueryHandler : IRequestHandler<GetProductDetailsByIdQuery, IDataResult<List<ProductDetailsViewModel>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediator _mediator;

        public GetProductDetailsByIdQueryHandler(IProductRepository productRepository, IMediator mediator)
        {
            _productRepository = productRepository;
            _mediator = mediator;
        }

        public async Task<IDataResult<List<ProductDetailsViewModel>>> Handle(GetProductDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _productRepository.GetProductDetailsByIdAsync(request.Id);
            if (result == null || result.Count == 0)
            {
                return new ErrorDataResult<List<ProductDetailsViewModel>>(Messages.DataNotFound);
            }

            _mediator.Publish(new ProductViewedEvent(request.Id));
            return new SuccessDataResult<List<ProductDetailsViewModel>>(result);
        }
    }
}