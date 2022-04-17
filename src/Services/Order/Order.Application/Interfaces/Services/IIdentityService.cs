using System;
using System.Threading.Tasks;

namespace Order.Application.Interfaces.Services
{
    public interface IIdentityService
    {
        Task<Guid> GetUserIdAsync();
    }
}