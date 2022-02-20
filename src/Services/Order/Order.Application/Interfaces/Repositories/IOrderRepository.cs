
namespace Order.Application.Interfaces.Repositories
{
    public interface IOrderRepository : IEntityRepository<Domain.AggregateModels.OrderAggregate.Order>
    {
    }
}