using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Order.Domain.SeedWork;

namespace Order.Domain.AggregateModels.OrderAggregate
{
    public class OrderItem : BaseEntity, IValidatableObject
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal UnitPrice { get; set; }
        public int Units { get; set; }

        public OrderItem()
        {
        }

        public OrderItem(string productId, string productName, string pictureUrl, decimal unitPrice, int units)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
            UnitPrice = unitPrice;
            Units = units;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Units < 1)
            {
                yield return new ValidationResult("Invalid number of units", new[] {"Units"});
            }
        }
    }
}