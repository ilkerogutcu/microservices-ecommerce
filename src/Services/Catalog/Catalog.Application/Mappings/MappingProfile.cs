using System;
using System.Linq.Expressions;
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
using Catalog.Application.Features.Commands.Products.CreateManyProductsCommand;
using Catalog.Domain.Entities;
using Google.Protobuf.WellKnownTypes;
using Media.Grpc.Protos;
using Option = Catalog.Domain.Entities.Option;

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
                .ForMember(dest => dest.Option, opt => opt.MapFrom(src => src.Option))
                .ReverseMap();
            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Domain.Entities.Media, MediaModel>()
                .Ignore(dest => dest.CreatedTimestamp)
                .ReverseMap();
        }
    }
}