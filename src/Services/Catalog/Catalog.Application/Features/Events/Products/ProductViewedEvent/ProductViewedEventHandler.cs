using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Interfaces.Repositories;
using MediatR;

namespace Catalog.Application.Features.Events.Products.ProductViewedEvent
{
    public class ProductViewedEventHandler : INotificationHandler<ProductViewedEvent>
    {
        private readonly IProductRepository _productRepository;

        public ProductViewedEventHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(ProductViewedEvent notification, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(notification.ProductId);
            product.ReviewsCount++;
            await _productRepository.UpdateAsync(product.Id, product);
        }
    }
}