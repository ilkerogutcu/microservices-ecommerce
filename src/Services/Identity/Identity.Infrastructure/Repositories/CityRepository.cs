using Identity.Application.Interfaces.Repositories;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence;

namespace Identity.Infrastructure.Repositories
{
    public class CityRepository: EfEntityRepositoryBase<City, IdentityContext>, ICityRepository
    {
        public CityRepository(IdentityContext context) : base(context)
        {
        }
    }
}