using System.Threading.Tasks;
using AutoMapper;
using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using Identity.Grpc;

namespace Catalog.Infrastructure.GrpcServices
{
    public class UserGrpcService : IUserGrpcService
    {
        private readonly IMapper _mapper;
        private readonly UserProtoService.UserProtoServiceClient _userProtoService;

        public UserGrpcService(IMapper mapper, UserProtoService.UserProtoServiceClient userProtoService)
        {
            _mapper = mapper;
            _userProtoService = userProtoService;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            var request = new GetUserByIdRequest { UserId = id };
            var userResponse = await _userProtoService.GetUserByIdAsync(request);
            var result = _mapper.Map<User>(userResponse);
            return result;
        }
    }
}