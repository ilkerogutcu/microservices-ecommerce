using Identity.Grpc.Entities;
using Identity.Grpc.Interfaces;
using Identity.Grpc.Persistence;

namespace Identity.Grpc.Repositories
{
    public class AddressRepository: EfEntityRepositoryBase<Address, IdentityContext>, IAddressRepository
    {
        public AddressRepository(IdentityContext context) : base(context)
        {
        }
    }
}