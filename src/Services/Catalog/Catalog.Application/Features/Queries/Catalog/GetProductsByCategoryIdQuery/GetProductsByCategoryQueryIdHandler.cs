using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Constants;
using Catalog.Application.Features.Queries.Catalog.ViewModels;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Wrappers;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.Catalog.GetProductsByCategoryIdQuery
{
    public class GetProductsByCategoryIdQueryHandler : IRequestHandler<GetProductsByCategoryIdQuery, IDataResult<List<ProductCardViewModel>>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsByCategoryIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IDataResult<List<ProductCardViewModel>>> Handle(GetProductsByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetProductsByCategoryIdAsync(request);
            if (products == null)
            {
                return new ErrorDataResult<List<ProductCardViewModel>>(Messages.DataNotFound);
            }

            return new PaginatedResult<List<ProductCardViewModel>>(products, request.PageIndex, request.PageSize, products.Count);
        }
    }
}