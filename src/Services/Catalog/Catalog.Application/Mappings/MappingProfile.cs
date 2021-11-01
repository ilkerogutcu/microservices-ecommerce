using AutoMapper;
using Catalog.Application.Dtos;
using Catalog.Application.Features.Commands.CreateBrandCommand;
using Catalog.Application.Features.Commands.CreateOptionCommand;
using Catalog.Application.Features.Commands.UpdateBrandCommand;
using Catalog.Domain.Entities;

namespace Catalog.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Brand, BrandDto>().ReverseMap();
            CreateMap<Brand, CreateBrandCommand>().ReverseMap();
            CreateMap<Brand, UpdateBrandCommand>().ReverseMap();

            CreateMap<Option, CreateOptionCommand>().ReverseMap();
        }
    }
}