using AutoMapper;
using Identity.Application.Features.Commands.Users.CreateUserCommand;
using Identity.Application.Features.Commands.Users.SignUpCommand;
using Identity.Domain.Entities;

namespace Identity.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, SignUpCommand>().ReverseMap();
            CreateMap<User, CreateUserCommand>().ReverseMap();

        }
    }
}