﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Basket.API.Core.Domain.Models
{
    public class BasketItem : IValidatableObject
    {
        [JsonIgnore] public string Id { get; set; }
        public string ProductId { get; set; }
        public string BrandName { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal UnitPrice { get; set; }
        public string Color { get; set; }
        public string HexCode { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(ProductId) || string.IsNullOrEmpty(BrandName) || string.IsNullOrEmpty(ProductName) ||
                string.IsNullOrEmpty(PictureUrl) || UnitPrice < 0 || Quantity < 1)
            {
                yield return new ValidationResult("Invalid basket item", new[] {"BasketItem"});
            }
        }
    }
}