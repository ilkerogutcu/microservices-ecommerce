using System.Threading.Tasks;
using Grpc.Core;
using Identity.Grpc.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.Grpc.Services
{
    public class UserService : UserProtoService.UserProtoServiceBase
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public override async Task<UserResponse> GetUserById(GetUserByIdRequest request, ServerCallContext context)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null)
            {
                return null;
            }

            return new UserResponse
            {
                UserId = user.Id,
                Email = user.Email,
                Phone = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }
    }
}