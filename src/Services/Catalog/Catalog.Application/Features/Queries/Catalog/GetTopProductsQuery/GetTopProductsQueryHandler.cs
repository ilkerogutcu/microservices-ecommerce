using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Constants;
using Catalog.Application.Features.Queries.Catalog.ViewModels;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.Catalog.GetTopProductsQuery
{
    public class GetTopProductsQueryHandler : IRequestHandler<GetTopProductsQuery, IDataResult<List<ProductCardViewModel>>>
    {
        private readonly IProductRepository _productRepository;

        public GetTopProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        public async Task<IDataResult<List<ProductCardViewModel>>> Handle(GetTopProductsQuery request,
            CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetTopProductsAsync(request.Count);
            if (products == null || products.Count == 0)
            {
                return new ErrorDataResult<List<ProductCardViewModel>>(Messages.DataNotFound);
            }

            return new SuccessDataResult<List<ProductCardViewModel>>(products);
        }
    }
}