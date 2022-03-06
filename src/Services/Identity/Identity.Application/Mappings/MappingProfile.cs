using AutoMapper;
using Identity.Application.Features.Commands.Users.CreateUserCommand;
using Identity.Application.Features.Commands.Users.SignUpCommand;
using Identity.Application.Features.Commands.Users.UpdateAddressCommand;
using Identity.Application.Features.Queries.Users.ViewModels;
using Identity.Application.Features.Queries.ViewModels;
using Identity.Domain.Entities;

namespace Identity.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, SignUpCommand>().ReverseMap();
            CreateMap<User, CreateUserCommand>().ReverseMap();
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<Address, AddressViewModel>()
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.District.City.Id))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.District.City.Name))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District.Name))
                .ReverseMap();
            CreateMap<Address, UpdateAddressFromUserCommand>().ReverseMap();
        }
    }
}