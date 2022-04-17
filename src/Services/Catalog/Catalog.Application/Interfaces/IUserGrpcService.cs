using System.Threading.Tasks;
using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces
{
    public interface IUserGrpcService
    {
        Task<User> GetUserByIdAsync(string id);
    }
}