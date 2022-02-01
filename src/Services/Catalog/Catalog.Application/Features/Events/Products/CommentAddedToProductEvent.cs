using MediatR;

namespace Catalog.Application.Features.Events.Products
{
    public class CommentAddedToProductEvent : INotification
    {
        public CommentAddedToProductEvent(string productId, double average)
        {
            ProductId = productId;
            Average = average;
        }

        public string ProductId { get; }
        public double Average { get; }
    }
}