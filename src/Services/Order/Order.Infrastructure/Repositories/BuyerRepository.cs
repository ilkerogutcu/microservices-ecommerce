using Order.Application.Interfaces.Repositories;
using Order.Domain.AggregateModels.BuyerAggregate;
using Order.Infrastructure.Context;

namespace Order.Infrastructure.Repositories
{
    public class BuyerRepository : EfEntityRepositoryBase<Buyer, OrderDbContext>, IBuyerRepository
    {
        public BuyerRepository(OrderDbContext context) : base(context)
        {
        }
    }
}