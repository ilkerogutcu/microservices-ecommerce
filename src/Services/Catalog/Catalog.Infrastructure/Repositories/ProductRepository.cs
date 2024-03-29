﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Application.Features.Queries.Catalog.GetProductsByCategoryIdQuery;
using Catalog.Application.Features.Queries.Catalog.ViewModels;
using Catalog.Application.Features.Queries.Products.GetAllProductsQuery;
using Catalog.Application.Helpers;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Utilities;
using Catalog.Domain.Entities;
using Catalog.Domain.Enums;
using Catalog.Infrastructure.Persistence;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : MongoDbRepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(ICatalogContext context) : base(context)
        {
        }

        public async Task<List<ProductDto>> GetAllProductsAsync(GetAllProductsQuery query)
        {
            var startDate = query.StartDate == 0
                ? DateTime.MinValue
                : HelperFunctions.ConvertTimestampToDateTime(query.StartDate);
            var endDate = query.StartDate == 0
                ? DateTime.Now
                : HelperFunctions.ConvertTimestampToDateTime(query.EndDate);

            var options = new CatalogContext().Options.AsQueryable();

            var colorOption = options.FirstOrDefault(x => x.NormalizedName.Contains("renk"));
            var sizeOption = options.FirstOrDefault(x => x.NormalizedName.Contains("beden"));

            var result = (from product in Collection.AsQueryable().ToList()
                    where product.Approved == query.Approved && product.Locked == query.Locked &&
                          product.IsActive == query.IsActive && product.CreatedDate >= startDate &&
                          product.CreatedDate <= endDate
                    select new ProductDto
                    {
                        ProductId = product.Id,
                        CategoryName = product.Category.Name,
                        CategoryId = product.Category.Id,
                        BrandName = product.Brand.Name,
                        BrandId = product.Brand.Id,
                        ThumbnailImageUrl = product.ThumbnailImageUrl,
                        Name = product.Name,
                        NormalizedName = product.NormalizedName,
                        ShortDescription = product.ShortDescription,
                        LongDescription = product.LongDescription,
                        ModelCode = product.ModelCode,
                        ReviewsCount = product.ReviewsCount,
                        RatingAverage = product.RatingAverage,
                        RatingCount = product.RatingCount,
                        IsActive = product.IsActive,
                        Barcode = product.Barcode,
                        StockCode = product.StockCode,
                        StockQuantity = product.StockQuantity,
                        SalePrice = product.SalePrice,
                        ListPrice = product.ListPrice,
                        IsFreeShipping = product.IsFreeShipping,
                        Approved = product.Approved,
                        Locked = product.Locked,
                        Color = product.OptionValues.FirstOrDefault(x => x.OptionId.Equals(colorOption?.Id))?.Name,
                        HexCode = ColorUtils.ToHexCode(product.OptionValues.FirstOrDefault(x => x.OptionId.Equals(colorOption?.Id))?.Name),
                        Size = product.OptionValues.FirstOrDefault(x => x.OptionId.Equals(sizeOption?.Id))?.Name,
                        CreatedBy = product.CreatedBy,
                        CreatedDate = new DateTimeOffset(product.CreatedDate).ToUnixTimeMilliseconds(),
                        LastUpdatedBy = product.LastUpdatedBy,
                        LastUpdatedTime = product.LastUpdatedDate.HasValue
                            ? new DateTimeOffset(product.LastUpdatedDate.Value).ToUnixTimeMilliseconds()
                            : null,
                        OptionValues = (
                            from productOptionValue in product.OptionValues
                            from option in options
                            where option.Id.Equals(productOptionValue.OptionId)
                            select new OptionValueDetailsDto
                            {
                                OptionId = option.Id,
                                OptionName = option.Name,
                                OptionValueId = productOptionValue.Id,
                                OptionValueName = productOptionValue.Name
                            }).ToList()
                    }).OrderBy(x => x.CreatedDate)
                .Skip(query.PageSize * query.PageIndex)
                .Take(query.PageSize);


            if (!string.IsNullOrEmpty(query.ModelCode)) result = result.Where(x => x.ModelCode.Equals(query.ModelCode));

            if (!string.IsNullOrEmpty(query.Barcode)) result = result.Where(x => x.Barcode.Equals(query.Barcode));

            if (!string.IsNullOrEmpty(query.StockCode)) result = result.Where(x => x.StockCode.Equals(query.StockCode));

            if (!string.IsNullOrEmpty(query.ProductName))
                result = result.Where(x => x.NormalizedName.Contains(query.ProductName.ToLower()));

            return await Task.FromResult(result.ToList());
        }

        public async Task<List<ProductCardViewModel>> GetTopProductsAsync(int count)
        {
            var result = (from product in (await Collection.FindAsync(x => x.Locked == false && x.IsActive && x.Approved)).ToList()
                    .Take(count).OrderBy(x => x.ReviewsCount)
                select new ProductCardViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Brand = product.Brand.Name,
                    BrandId = product.Brand.Id,
                    ThumbnailImageUrl = product.ThumbnailImageUrl,
                    ShortDescription = product.ShortDescription,
                    ReviewsCount = product.ReviewsCount,
                    RatingAverage = product.RatingAverage,
                    Barcode = product.Barcode,
                    StockQuantity = product.StockQuantity,
                    SalePrice = product.SalePrice,
                    ListPrice = product.ListPrice,
                    IsFreeShipping = product.IsFreeShipping,
                    DiscountRate = Convert.ToInt32((product.SalePrice - product.ListPrice) / product.ListPrice * 100),
                }).ToList();
            return result;
        }

        public async Task<List<ProductCardViewModel>> GetProductsByCategoryIdAsync(GetProductsByCategoryIdQuery query)
        {
            var result = (from product in (await Collection.FindAsync(x =>
                        x.Category.Id == query.CategoryId && x.Locked == false && x.IsActive && x.Approved)).ToList()
                    .Skip(query.PageSize * query.PageIndex).Take(query.PageSize)
                select new ProductCardViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Brand = product.Brand.Name,
                    BrandId = product.Brand.Id,
                    ThumbnailImageUrl = product.ThumbnailImageUrl,
                    ShortDescription = product.ShortDescription,
                    ReviewsCount = product.ReviewsCount,
                    RatingAverage = product.RatingAverage,
                    Barcode = product.Barcode,
                    StockQuantity = product.StockQuantity,
                    SalePrice = product.SalePrice,
                    ListPrice = product.ListPrice,
                    IsFreeShipping = product.IsFreeShipping,
                    DiscountRate = Convert.ToInt32((product.SalePrice - product.ListPrice) / product.ListPrice * 100),
                }).ToList();

            switch (query.SortBy)
            {
                case SortBy.PriceByAsc:
                    result = result.OrderBy(x => x.ListPrice).ToList();
                    break;
                case SortBy.PriceByDesc:
                    result = result.OrderByDescending(x => x.ListPrice).ToList();
                    break;

                case SortBy.MostRecent:
                    result = result.OrderByDescending(x => x.ReviewsCount).ToList();
                    break;
                case SortBy.BestSeller:
                default:
                    result = result.OrderByDescending(x => x.ReviewsCount).ToList();
                    break;
            }

            return result;
        }

        public async Task<List<ProductDetailsViewModel>> GetProductDetailsByIdAsync(string id)
        {
            var modelCode = Collection.Find(x => x.Id.Equals(id) && x.Locked == false && x.IsActive && x.Approved).FirstOrDefault()?.ModelCode;
            var options = new CatalogContext().Options.AsQueryable();

            var colorOption = options.FirstOrDefault(x => x.NormalizedName.Contains("renk"));
            var sizeOption = options.FirstOrDefault(x => x.NormalizedName.Contains("beden"));

            var result = (from product in (await Collection.FindAsync(x =>
                    x.ModelCode.Equals(modelCode) && x.Locked == false && x.IsActive && x.Approved)).ToList()
                select new ProductDetailsViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Brand = product.Brand.Name,
                    BrandId = product.Brand.Id,
                    ModelCode = modelCode,
                    RatingAverage = product.RatingAverage ?? 0,
                    RatingCount = product.RatingCount,
                    LongDescription = product.LongDescription,
                    ShortDescription = product.ShortDescription,
                    Barcode = product.Barcode,
                    SalePrice = product.SalePrice,
                    ListPrice = product.ListPrice,
                    DiscountRate = Convert.ToInt32((product.SalePrice - product.ListPrice) / product.ListPrice * 100),
                    IsFreeShipping = product.IsFreeShipping,
                    Color = product.OptionValues.FirstOrDefault(x => x.OptionId.Equals(colorOption?.Id))?.Name,
                    HexCode = ColorUtils.ToHexCode(product.OptionValues.FirstOrDefault(x => x.OptionId.Equals(colorOption?.Id))?.Name),
                    Size = product.OptionValues.FirstOrDefault(x => x.OptionId.Equals(sizeOption?.Id))?.Name,
                    StockQuantity = product.StockQuantity,
                    ImageUrls = product.ImageUrls.ToList(),
                    OptionValues = (from optionValue in product.OptionValues
                        join option in options on optionValue.OptionId equals option.Id
                        select new OptionValueDetailsDto
                        {
                            OptionId = option.Id,
                            OptionName = option.Name,
                            OptionValueId = optionValue.Id,
                            OptionValueName = optionValue.Name,
                        }).ToList()
                }).ToList();
            return result;
        }
    }
}