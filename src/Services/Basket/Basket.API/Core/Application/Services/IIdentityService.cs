using System;
using System.Threading.Tasks;

namespace Basket.API.Core.Application.Services
{
    public interface IIdentityService
    {
        Task<Guid> GetUserIdAsync();
    }
}