using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using Catalog.Domain.Entities;
using MongoDB.Driver;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Infrastructure.Persistence
{
    public class CatalogContextSeed
    {
        public static void SeedBrandData(IMongoCollection<Brand> brandCollection)
        {
            var existBrand = brandCollection.Find(x => true).Any();
            if (!existBrand) brandCollection.InsertManyAsync(GetPreconfiguredBrands());
        }

        public static void SeedOptionData(IMongoCollection<Option> optionCollection)
        {
            var existOption = optionCollection.Find(x => true).Any();
            if (!existOption) optionCollection.InsertManyAsync(GetPreconfiguredOptions());
        }

        public static void SeedOptionValueData(IMongoCollection<OptionValue> optionValueCollection,
            IMongoCollection<Option> optionCollection)
        {
            var existOptionValue = optionValueCollection.Find(x => true).Any();
            if (!existOptionValue)
                optionValueCollection.InsertManyAsync(GetPreconfiguredOptionValues(optionCollection));
        }

        public static void SeedCategoryData(IMongoCollection<Category> categoryCollection)
        {
            var existCategory = categoryCollection.Find(x => true).Any();
            if (!existCategory) categoryCollection.InsertOneAsync(GetPreconfiguredCategories());
        }

        public static void SeedCategoryOptionValueData(
            IMongoCollection<CategoryOptionValue> categoryOptionValueCollection,
            IMongoCollection<OptionValue> optionValueCollection, IMongoCollection<Category> categoryCollection)
        {
            var existCategoryOptionValue = categoryOptionValueCollection.Find(x => true).Any();
            if (!existCategoryOptionValue)
                categoryOptionValueCollection.InsertManyAsync(
                    GetPreconfiguredCategoryOptionValues(categoryCollection, optionValueCollection));
        }

        public static void SeedProductData(IMongoCollection<Product> productCollection,
            IMongoCollection<CategoryOptionValue> categoryOptionValue,
            IMongoCollection<Brand> brandCollection)
        {
            var existProduct = productCollection.Find(x => true).Any();
            if (!existProduct)
            {
                productCollection.InsertManyAsync(GetPreconfiguredProducts(categoryOptionValue, brandCollection));
            }
        }

        private static IEnumerable<Brand> GetPreconfiguredBrands()
        {
            return new List<Brand>
            {
                new()
                {
                    Id = "102d2149e773f2a3990b47f1",
                    Name = "Adidas",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now,
                    IsActive = true
                },
                new()
                {
                    Id = "102d2149e773f2a3990b47f12",
                    Name = "Nike",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now,
                    IsActive = true
                },
                new()
                {
                    Id = "102d2149e773f2a3990b47f3",
                    Name = "Puma",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now,
                    IsActive = true
                },
                new()
                {
                    Id = "102d2149e773f2a3990b47f4",
                    Name = "Apple",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now,
                    IsActive = true
                }
            };
        }

        private static IEnumerable<Option> GetPreconfiguredOptions()
        {
            return new List<Option>
            {
                new()
                {
                    Id = "202d2149e773f2a3990b47f1",
                    Name = "Beden",
                    IsRequired = true,
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now,
                    Varianter = true
                },
                new()
                {
                    Id = "202d2149e773f2a3990b47f2",
                    Name = "Materyal",
                    IsRequired = true,
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now,
                    Varianter = false
                },
                new()
                {
                    Id = "202d2149e773f2a3990b47f4",
                    Name = "Taban",
                    IsRequired = false,
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now,
                    Varianter = false
                },
                new()
                {
                    Id = "202d2149e773f2a3990b47f5",
                    Name = "Yaş Grubu",
                    IsRequired = true,
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now,
                    Varianter = false
                },
                new()
                {
                    Id = "202d2149e773f2a3990b47f6",
                    Name = "Cinsiyet",
                    IsRequired = true,
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now,
                    Varianter = false
                }
            };
        }

        private static IEnumerable<OptionValue> GetPreconfiguredOptionValues(IMongoCollection<Option> optionCollection)
        {
            var option1 = optionCollection.Find(x => x.Id == "202d2149e773f2a3990b47f6").FirstOrDefault();
            var option2 = optionCollection.Find(x => x.Id == "202d2149e773f2a3990b47f5").FirstOrDefault();

            return new List<OptionValue>
            {
                new()
                {
                    Id = "302d2149e773f2a3990b47f1",
                    Option = option1,
                    Name = "Erkek",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now
                },
                new()
                {
                    Id = "302d2149e773f2a3990b47f2",
                    Option = option1,
                    Name = "Kadın / Kız",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now
                },
                new()
                {
                    Id = "302d2149e773f2a3990b47f3",
                    Option = option1,
                    Name = "Unisex",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now
                },
                new()
                {
                    Id = "302d2149e773f2a3990b47f4",
                    Option = option2,
                    Name = "Bebek",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now
                },
                new()
                {
                    Id = "302d2149e773f2a3990b47f5",
                    Option = option2,
                    Name = "Çocuk",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now
                },
                new()
                {
                    Id = "302d2149e773f2a3990b47f6",
                    Option = option2,
                    Name = "Genç",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now
                },
                new()
                {
                    Id = "302d2149e773f2a3990b47f7",
                    Option = option2,
                    Name = "Yetişkin",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now
                }
            };
        }

        private static Category GetPreconfiguredCategories()
        {
            var clothingMainCategory = new Category
            {
                Id = "402d2149e773f2a3990b47f1",
                Name = "Giyim",
                ParentId = null,
                CreatedBy = "admin",
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            clothingMainCategory.Nodes.Add(new Category
            {
                Id = "502d2149e773f2a3990b47f1",
                Name = "Gelinlik",
                ParentId = clothingMainCategory.Id,
                CreatedBy = "admin",
                CreatedDate = DateTime.Now,
                Nodes = new List<Category>()
                {
                    new Category
                    {
                        Id = "8902d2149e773f2a3990b47f2",
                        Name = "deneme gelinlik kategori",
                        ParentId = clothingMainCategory.Id,
                        CreatedBy = "admin",
                        CreatedDate = DateTime.Now,
                        IsActive = true
                    }
                },
                IsActive = true,
            });
            clothingMainCategory.Nodes.Add(new Category
            {
                Id = "502d2149e773f2a3990b47f2",
                Name = "Elbise",
                ParentId = clothingMainCategory.Id,
                CreatedBy = "admin",
                CreatedDate = DateTime.Now,
                IsActive = true
            });
            return clothingMainCategory;
        }
        

        private static List<CategoryOptionValue> GetPreconfiguredCategoryOptionValues(
            IMongoCollection<Category> categoryCollection, IMongoCollection<OptionValue> optionValueCollection)
        {
            // TODO fix
            //var asdasd = categoryCollection.c(x => true).ToList().CloneWhere(x=>x.Id=="502d2149e773f2a3990b47f1");
            var gelinlikCategory = categoryCollection.Find(x => x.Id == "c").FirstOrDefault();
            var elbiseCategory = categoryCollection.Find(x => x.Id == "502d2149e773f2a3990b47f2").FirstOrDefault();
            return new List<CategoryOptionValue>
            {
                new()
                {
                    Id = "308d2149e773f2a3990b47f2",
                    Category = gelinlikCategory,
                    OptionValue = optionValueCollection.Find(x => x.Id == "302d2149e773f2a3990b47f2").FirstOrDefault(),
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now,
                },
                new()
                {
                    Id = "389d2149e773f2a3990b47f2",
                    Category = elbiseCategory,
                    OptionValue = optionValueCollection.Find(x => x.Id == "302d2149e773f2a3990b47f2").FirstOrDefault()
                },

                new()
                {
                    Id = "378d2149e773f2a3990b47f2",
                    Category = elbiseCategory,
                    OptionValue = optionValueCollection.Find(x => x.Id == "302d2149e773f2a3990b47f4").FirstOrDefault()
                },
                new()
                {
                    Id = "328d2149e773f2a3990b47f2",
                    Category = elbiseCategory,
                    OptionValue = optionValueCollection.Find(x => x.Id == "302d2149e773f2a3990b47f5").FirstOrDefault()
                }
            };
        }

        private static List<Product> GetPreconfiguredProducts(IMongoCollection<CategoryOptionValue> categoryOptionValue,
            IMongoCollection<Brand> brandCollection)
        {

            var categoryOption = categoryOptionValue.Find(x => x.Category.Id == "502d2149e773f2a3990b47f2").ToList();
            return new List<Product>
            {
                new()
                {
                    Id = "702d2149e773f2a3990b47f2",
                    Name = "Çok Renkli Kareli Nakış Detaylı Elbise",
                    ShortDescription = "Dokuma kumaş elbise",
                    RatingAverage = 4.3,
                    ReviewsCount = 5,
                    ModelCode = "KARELINAKIS20",
                    LongDescription = "%50 Polyester %50 Viskon, Dokuma Kumaş",
                    Brand = brandCollection.Find(x => x.Id == "102d2149e773f2a3990b47f1").FirstOrDefault(),
                    ThumbnailMedia = new Media
                    {
                        Id = "902d2149e773f2a3990b47c1",
                        Url =
                            "https://cdn.dsmcdn.com/mnresize/1200/1800/ty83/product/media/images/20210323/16/74721633/156151688/1/1_org_zoom.jpg",
                        CreatedBy = "admin",
                        CreatedDate = DateTime.Now,
                        PublicId = "156151688"
                    },
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now,
                    Category = categoryOption[0].Category,
                    Comments = new List<Comment>
                    {
                        new()
                        {
                            Id = "892d2149e773f2a3990b47f1",
                            Content = "Ürün çok güzel, üzerime tam oldu.",
                            CreatedBy = "ayşe@gmail.com",
                            CreatedDate = DateTime.Now,
                            Rating = 4.8
                        },
                        new()
                        {
                            Id = "902d2149e773f2a3990b47f1",
                            Content = "Ürün kalitesi kötüydü.",
                            CreatedBy = "elif@gmail.com",
                            CreatedDate = DateTime.Now,
                            Rating = 1.5
                        }
                    },
                    Medias = new List<Media>
                    {
                        new()
                        {
                            Id = "902d2149e773f2a3990b47c1",
                            Url =
                                "https://cdn.dsmcdn.com/mnresize/1200/1800/ty83/product/media/images/20210323/16/74721633/156151688/1/1_org_zoom.jpg",
                            CreatedBy = "admin",
                            CreatedDate = DateTime.Now,
                            PublicId = "156151688"
                        },
                        new()
                        {
                            Id = "902d2149e773f2a3990b47q1",
                            Url =
                                "https://cdn.dsmcdn.com/mnresize/1200/1800/ty85/product/media/images/20210323/16/74721633/156151688/4/4_org_zoom.jpg",
                            CreatedBy = "admin",
                            CreatedDate = DateTime.Now,
                            PublicId = "74721633"
                        }
                    },
                    Skus = new List<Sku>
                    {
                        new()
                        {
                            Id = "902d2149e773f2a3995b47c1",
                            Approved = true,
                            Barcode = "ELBISE2122",
                            Locked = false,
                            SalePrice = 215,
                            ListPrice = 320,
                            StockCode = "ELBISESTOCK2121",
                            StockQuantity = 20,
                            OptionValues = new List<OptionValue>
                            {
                                categoryOption[0].OptionValue,
                                categoryOption[1].OptionValue
                            }
                        }
                    }
                }
            };
        }
    }
}