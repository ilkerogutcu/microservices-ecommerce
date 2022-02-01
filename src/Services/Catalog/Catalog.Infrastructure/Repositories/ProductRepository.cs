using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Application.Features.Queries.Catalog.ViewModels;
using Catalog.Application.Features.Queries.Products.GetAllProductsQuery;
using Catalog.Application.Helpers;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : MongoDbRepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(ICatalogContext<Product> context) : base(context)
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

            var options = new CatalogContext<Option>().Options.AsQueryable();

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
            var result=(from product in  (await Collection.FindAsync(x=>true)).ToList().OrderBy(x=>x.ReviewsCount).Take(count)
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
    }
}