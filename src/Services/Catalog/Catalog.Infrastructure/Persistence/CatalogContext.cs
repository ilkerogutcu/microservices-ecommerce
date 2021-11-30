using System;
using Catalog.Application.Models.Configs;
using Catalog.Domain.Common;
using Catalog.Domain.Entities;
using Catalog.Domain.Utilities.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Olcsan.Boilerplate.Utilities.IoC;

namespace Catalog.Infrastructure.Persistence
{
    public class CatalogContext<T> : ICatalogContext<T> where  T: BaseEntity
    {
        private readonly IMongoDatabase _database;
        public CatalogContext()
        {
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
            var config = configuration?.GetSection("MongoDatabaseConfig").Get<MongoDatabaseConfig>();
            ConnectionSettingControl(config);
            var client = new MongoClient(config?.ConnectionString);
            _database = client.GetDatabase(config?.DatabaseName);

            Brands = _database.GetCollection<Brand>("Brand");
            Categories = _database.GetCollection<Category>("Category");
            CategoryOptionValues = _database.GetCollection<CategoryOptionValue>("CategoryOptionValue");
            Comments = _database.GetCollection<Comment>("Comment");
            Medias = _database.GetCollection<Domain.Entities.Media>("Media");
            Options = _database.GetCollection<Option>("Option");
            OptionValues = _database.GetCollection<OptionValue>("OptionValue");
            Products = _database.GetCollection<Product>("Product");
            Skus = _database.GetCollection<Sku>("Sku");
            //
            // CatalogContextSeed.SeedBrandData(Brands);
            // CatalogContextSeed.SeedOptionData(Options);
            // CatalogContextSeed.SeedOptionValueData(OptionValues, Options);
            // CatalogContextSeed.SeedCategoryData(Categories);
            // CatalogContextSeed.SeedCategoryOptionValueData(CategoryOptionValues, OptionValues, Categories);
            // CatalogContextSeed.SeedProductData(Products, CategoryOptionValues, Brands);
        }

        private void ConnectionSettingControl(MongoDatabaseConfig config)
        {
            if (config != null && string.IsNullOrEmpty(config.DatabaseName))
                throw new Exception(MongoDbMessages.NullOrEmptyMessage);
        }

        public IMongoCollection<Brand> Brands { get; set; }
        public IMongoCollection<Category> Categories { get; set; }
        public IMongoCollection<CategoryOptionValue> CategoryOptionValues { get; set; }
        public IMongoCollection<Comment> Comments { get; set; }
        public IMongoCollection<Domain.Entities.Media> Medias { get; set; }
        public IMongoCollection<Option> Options { get; set; }
        public IMongoCollection<OptionValue> OptionValues { get; set; }
        public IMongoCollection<Product> Products { get; set; }
        public IMongoCollection<Sku> Skus { get; set; }
        public IMongoCollection<T> GetCollection()
        {
            return _database.GetCollection<T>(typeof(T).Name);
        }
    }
}