using System;
using System.Collections.Generic;

namespace Basket.API.Core.Domain.Models
{
    public class CustomerBasket
    {
        public Guid BuyerId { get; set; }
        public List<BasketItem> Items { get; set; }= new List<BasketItem>();

        public CustomerBasket()
        {
        }

        public CustomerBasket(Guid buyerId)
        {
            BuyerId = buyerId;
        }
    }
}