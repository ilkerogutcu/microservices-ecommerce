using System;
using AutoMapper;
using Catalog.Application.Dtos;
using Catalog.Application.Extensions;
using Catalog.Application.Features.Commands.Brands.CreateBrandCommand;
using Catalog.Application.Features.Commands.Brands.UpdateBrandCommand;
using Catalog.Application.Features.Commands.Categories.CreateCategoryCommand;
using Catalog.Application.Features.Commands.Options.CreateOptionCommand;
using Catalog.Application.Features.Commands.Options.UpdateOptionCommand;
using Catalog.Application.Features.Commands.OptionValues.CreateOptionValueCommand;
using Catalog.Application.Features.Commands.OptionValues.UpdateOptionValueCommand;
using Catalog.Application.Features.Commands.Products.UpdateProductCommand;
using Catalog.Domain.Entities;
using Identity.Grpc;
using Media.Grpc.Protos;

namespace Catalog.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Brand, BrandDto>().ReverseMap();
            CreateMap<Brand, CreateBrandCommand>().ReverseMap();
            CreateMap<Brand, UpdateBrandCommand>().ReverseMap();

            CreateMap<Option, OptionDto>().ReverseMap();
            CreateMap<Option, CreateOptionCommand>().ReverseMap();
            CreateMap<Option, UpdateOptionCommand>().ReverseMap();

            CreateMap<OptionValue, OptionValueDto>().ReverseMap();
            CreateMap<OptionValue, CreateOptionValueCommand>().ReverseMap();
            CreateMap<OptionValue, UpdateOptionValueCommand>().ReverseMap();

            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryCommand>().ReverseMap();
            CreateMap<CategoryOptionValue, CategoryOptionValueDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category.Id))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ReverseMap();

            CreateMap<Product, CreateProductDto>()
                .ReverseMap();
            CreateMap<Domain.Entities.Media, MediaModel>()
                .Ignore(dest => dest.CreatedTimestamp)
                .ReverseMap();
            CreateMap<Product, UpdateProductCommand>().ReverseMap();
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.Brand.Id))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category.Id))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.CreatedDate,
                    opt => opt.MapFrom(src => new DateTimeOffset(src.CreatedDate).ToUnixTimeMilliseconds()))
                .ForMember(dest => dest.LastUpdatedTime,
                    opt => opt.MapFrom(src =>
                        src.LastUpdatedDate.HasValue
                            ? new DateTimeOffset(src.LastUpdatedDate.Value).ToUnixTimeMilliseconds()
                            : (long?) null
                    ))
                .ReverseMap();
            
            CreateMap<Comment,CommentDto>().ReverseMap();
            CreateMap<User,UserResponse>().ReverseMap();

        }
    }
}