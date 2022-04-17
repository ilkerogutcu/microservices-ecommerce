using AutoMapper;
using Catalog.Grpc.Protos;
using Order.Application.Dtos;

namespace Order.Application.Mapping.CatalogMapping
{
    public class CatalogMappingProfile : Profile
    {
        public CatalogMappingProfile()
        {
            CreateMap<ProductResponse, ProductDetailsDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.BrandName))
                .ReverseMap();
        }
    }
}