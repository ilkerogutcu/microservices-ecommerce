using Identity.Application.Interfaces.Repositories;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence;

namespace Identity.Infrastructure.Repositories
{
    public class AddressRepository: EfEntityRepositoryBase<Address, IdentityContext>, IAddressRepository
    {
        public AddressRepository(IdentityContext context) : base(context)
        {
        }
    }
}