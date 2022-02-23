using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Grpc.Entities;
using Catalog.Grpc.Interfaces.Repositories;
using Catalog.Grpc.Persistence;
using Catalog.Grpc.Utilities;
using Catalog.Grpc.ViewModels;
using MongoDB.Driver;

namespace Catalog.Grpc.Repositories
{
    public class ProductRepository : MongoDbRepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(ICatalogContext<Product> context) : base(context)
        {
        }

        public async Task<ProductDetailsViewModel> GetProductDetailsByIdAsync(string id)
        {
            var options = new CatalogContext<Option>().Options.AsQueryable();

            var colorOption = options.FirstOrDefault(x => x.NormalizedName.Contains("renk"));
            var sizeOption = options.FirstOrDefault(x => x.NormalizedName.Contains("beden"));

            var result = (from product in (await Collection.FindAsync(x =>
                    x.Id.Equals(id) && x.Locked == false && x.IsActive && x.Approved)).ToList()
                select new ProductDetailsViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Brand = product.Brand.Name,
                    BrandId = product.Brand.Id,
                    ModelCode = product.ModelCode,
                    Barcode = product.Barcode,
                    SalePrice = product.SalePrice,
                    Color = product.OptionValues.FirstOrDefault(x => x.OptionId.Equals(colorOption?.Id))?.Name,
                    HexCode = ColorUtils.ToHexCode(product.OptionValues.FirstOrDefault(x => x.OptionId.Equals(colorOption?.Id))?.Name),
                    Size = product.OptionValues.FirstOrDefault(x => x.OptionId.Equals(sizeOption?.Id))?.Name,
                    StockQuantity = product.StockQuantity,
                }).FirstOrDefault();
            return result;
        }
    }
}