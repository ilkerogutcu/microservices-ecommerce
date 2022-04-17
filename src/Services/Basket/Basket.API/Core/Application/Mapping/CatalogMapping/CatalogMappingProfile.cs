using AutoMapper;
using Basket.API.Core.Application.Dtos;
using Basket.API.Core.Domain.Models;
using Catalog.Grpc.Protos;

namespace Basket.API.Core.Application.Mapping.CatalogMapping
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