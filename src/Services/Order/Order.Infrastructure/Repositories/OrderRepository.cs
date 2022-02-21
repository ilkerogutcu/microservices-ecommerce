using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Order.Application.Interfaces.Repositories;
using Order.Infrastructure.Context;

namespace Order.Infrastructure.Repositories
{
    public class OrderRepository : EfEntityRepositoryBase<Domain.AggregateModels.OrderAggregate.Order, OrderDbContext>, IOrderRepository
    {
        public OrderRepository(OrderDbContext context) : base(context)
        {
        }

        public async Task<Domain.AggregateModels.OrderAggregate.Order> GetByIdAsync(Guid id,
            params Expression<Func<Domain.AggregateModels.OrderAggregate.Order, object>>[] includes)
        {
            var entity = await base.GetAsync(x => x.Id == id, includes) ?? Context.Orders.Local.FirstOrDefault(x => x.Id == id);

            return entity;
        }
    }
}