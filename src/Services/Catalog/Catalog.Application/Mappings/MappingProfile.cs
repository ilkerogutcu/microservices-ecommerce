﻿using AutoMapper;
using Catalog.Application.Dtos;
using Catalog.Application.Features.Commands.Brands.CreateBrandCommand;
using Catalog.Application.Features.Commands.Brands.UpdateBrandCommand;
using Catalog.Application.Features.Commands.Categories.CreateCategoryCommand;
using Catalog.Application.Features.Commands.Options.CreateOptionCommand;
using Catalog.Application.Features.Commands.Options.UpdateOptionCommand;
using Catalog.Application.Features.Commands.OptionValues.CreateOptionValueCommand;
using Catalog.Application.Features.Commands.OptionValues.UpdateOptionValueCommand;
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

            CreateMap<Option, OptionDto>().ReverseMap();
            CreateMap<Option, CreateOptionCommand>().ReverseMap();
            CreateMap<Option, UpdateOptionCommand>().ReverseMap();

            CreateMap<OptionValue, CreateOptionValueCommand>().ReverseMap();
            CreateMap<OptionValue, UpdateOptionValueCommand>().ReverseMap();
            
            CreateMap<Category, CreateCategoryCommand>().ReverseMap();
        }
    }
}