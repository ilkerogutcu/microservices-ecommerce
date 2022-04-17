using System;
using Catalog.Grpc.Common;
using Catalog.Grpc.Configs;
using Catalog.Grpc.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Olcsan.Boilerplate.Utilities.IoC;

namespace Catalog.Grpc.Persistence
{
    public class CatalogContext<T> : ICatalogContext<T> where T : BaseEntity
    {
        private readonly IMongoDatabase _database;

        public CatalogContext()
        {
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
            var config = configuration?.GetSection("DatabaseSettings").Get<MongoDatabaseConfig>();
            Console.WriteLine("Connection string:" + config.ConnectionString);
            Console.WriteLine($"collection. {config.DatabaseName}");
            ConnectionSettingControl(config);
            var client = new MongoClient(config?.ConnectionString);
            _database = client.GetDatabase(config?.DatabaseName);

            Brands = _database.GetCollection<Brand>("Brand");
            Categories = _database.GetCollection<Category>("Category");
            CategoryOptionValues = _database.GetCollection<CategoryOptionValue>("CategoryOptionValue");
            Comments = _database.GetCollection<Comment>("Comment");
            Options = _database.GetCollection<Option>("Option");
            OptionValues = _database.GetCollection<OptionValue>("OptionValue");
            Products = _database.GetCollection<Product>("Product");
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
                throw new Exception("Database name is not defined");
        }

        public IMongoCollection<Brand> Brands { get; set; }
        public IMongoCollection<Category> Categories { get; set; }
        public IMongoCollection<CategoryOptionValue> CategoryOptionValues { get; set; }
        public IMongoCollection<Comment> Comments { get; set; }
        public IMongoCollection<Option> Options { get; set; }
        public IMongoCollection<OptionValue> OptionValues { get; set; }
        public IMongoCollection<Product> Products { get; set; }

        public IMongoCollection<T> GetCollection()
        {
            return _database.GetCollection<T>(typeof(T).Name);
        }
    }
}