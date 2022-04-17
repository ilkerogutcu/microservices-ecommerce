using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Olcsan.Boilerplate.Aspects.Autofac.Exception;
using Olcsan.Boilerplate.Aspects.Autofac.Logger;
using Olcsan.Boilerplate.CrossCuttingConcerns.Logging.Serilog.Loggers;

namespace Catalog.Application.Features.Events.Products.CommentAddedToProductEvent
{
    public class CommentAddedToProductEventHandler : INotificationHandler<CommentAddedToProductEvent>
    {
        private readonly IProductRepository _productRepository;

        public CommentAddedToProductEventHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [LogAspect(typeof(FileLogger), "Catalog-Application")]
        [ExceptionLogAspect(typeof(FileLogger), "Catalog-Application")]
        public async Task Handle(CommentAddedToProductEvent notification, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(notification.ProductId);
            if (product is null)
            {
                return;
            }

            var starCounts = product.Comments.GroupBy(x => x.Rating)
                .Select(x => new {Value = x.Key, Count = x.Count()});
            double newRating = starCounts.Aggregate(0, (current, starCount) => current + starCount.Value * starCount.Count);

            newRating /= product.RatingCount + 1;
            product.RatingAverage = newRating;
            product.RatingCount += 1;
            await _productRepository.UpdateAsync(product.Id, product);
        }
    }
}