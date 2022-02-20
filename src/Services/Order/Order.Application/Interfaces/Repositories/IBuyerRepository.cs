using Order.Domain.AggregateModels.BuyerAggregate;

namespace Order.Application.Interfaces.Repositories
{
    public interface IBuyerRepository : IEntityRepository<Buyer>
    {
    }
}