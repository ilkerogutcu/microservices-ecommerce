using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Order.Application.Interfaces.Repositories
{
    public interface IOrderRepository : IEntityRepository<Domain.AggregateModels.OrderAggregate.Order>
    {
        Task<Domain.AggregateModels.OrderAggregate.Order> GetByIdAsync(Guid id,
            params Expression<Func<Domain.AggregateModels.OrderAggregate.Order, object>>[] includes);
    }
}