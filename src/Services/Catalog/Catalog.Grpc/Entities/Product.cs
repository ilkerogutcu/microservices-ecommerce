using System;
using System.Collections.Generic;
using Catalog.Grpc.Common;

namespace Catalog.Grpc.Entities
{
    public class Product : BaseEntity
    {
        public Category Category { get; set; }
        public Brand Brand { get; set; }

        public string ThumbnailImageUrl { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string ModelCode { get; set; }
        public int ReviewsCount { get; set; }
        public double? RatingAverage { get; set; }
        public int RatingCount { get; set; }
        public bool IsActive { get; set; }

        public string Barcode { get; set; }
        public string StockCode { get; set; }
        public int StockQuantity { get; set; }
        public decimal SalePrice { get; set; }
        public decimal? ListPrice { get; set; }
        public bool IsFreeShipping { get; set; }
        public bool Approved { get; set; }
        public bool Locked { get; set; }
        public List<OptionValue> OptionValues { get; set; } = new();
        public IList<string> ImageUrls { get; set; } = new List<string>();
        public IList<Comment> Comments { get; set; } = new List<Comment>();

        public void AddOptionValue(OptionValue optionValue)
        {
            OptionValues.Add(optionValue);
        }

        public void AddOptionValues(List<OptionValue> optionValues)
        {
            OptionValues.AddRange(optionValues);
        }

        public void AddImageUrl(string imageUrl)
        {
            ImageUrls.Add(imageUrl);
        }

        public void AddComment(Comment comment)
        {
            comment.CreatedDate = DateTime.Now;
            Comments.Add(comment);
        }
    }
}