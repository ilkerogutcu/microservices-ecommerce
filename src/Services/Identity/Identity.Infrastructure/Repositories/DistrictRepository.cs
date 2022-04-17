using Identity.Application.Interfaces.Repositories;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence;

namespace Identity.Infrastructure.Repositories
{
    public class DistrictRepository : EfEntityRepositoryBase<District, IdentityContext>, IDistrictRepository
    {
        public DistrictRepository(IdentityContext context) : base(context)
        {
        }
    }
}