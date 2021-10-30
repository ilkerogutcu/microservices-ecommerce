using AutoMapper;
using Catalog.Application.Dtos;
using Catalog.Domain.Entities;

namespace Catalog.Application.Mappings
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Brand, BrandDto>().ReverseMap();
        }
    }
}