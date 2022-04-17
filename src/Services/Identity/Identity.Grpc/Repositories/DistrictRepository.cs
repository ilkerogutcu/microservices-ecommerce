using Identity.Grpc.Entities;
using Identity.Grpc.Interfaces;
using Identity.Grpc.Persistence;

namespace Identity.Grpc.Repositories
{
    public class DistrictRepository: EfEntityRepositoryBase<District, IdentityContext>, IDistrictRepository
    {
        public DistrictRepository(IdentityContext context) : base(context)
        {
        }
    }
}