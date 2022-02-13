using MediatR;

namespace Catalog.Application.Features.Events.Products.ProductViewedEvent
{
    public class ProductViewedEvent : INotification
    {
        public string ProductId { get; }

        public ProductViewedEvent(string productId)
        {
            ProductId = productId;
        }
    }
}