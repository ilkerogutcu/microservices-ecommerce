using Order.Domain.SeedWork;

namespace Order.Domain.AggregateModels.BuyerAggregate
{
    public class CardType : Enumeration
    {
        public static CardType Amex = new CardType(1, "Amex");
        public static CardType Visa = new CardType(2, "Visa");
        public static CardType MasterCard = new CardType(3, "MasterCard");
        public static CardType Discover = new CardType(4, "Discover");

        public CardType(int id, string name) : base(id, name)
        {
        }
    }
}