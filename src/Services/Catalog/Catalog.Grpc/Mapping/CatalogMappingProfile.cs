using AutoMapper;
using Catalog.Grpc.Protos;
using Catalog.Grpc.ViewModels;

namespace Catalog.Grpc.Mapping
{
    public class CatalogMappingProfile : Profile
    {
        public CatalogMappingProfile()
        {
            CreateMap<ProductResponse, ProductDetailsViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.BrandName))
                .ReverseMap();
        }
    }
}