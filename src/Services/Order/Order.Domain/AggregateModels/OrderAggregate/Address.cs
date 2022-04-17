namespace Order.Domain.AggregateModels.OrderAggregate
{
    public record Address(string FirstName, string LastName, string PhoneNumber, string City, string Zip, string District, string AddressLine,
        string AddressTitle)
    {
        public string FirstName { get; set; } = FirstName;
        public string LastName { get; set; } = LastName;
        public string PhoneNumber { get; set; } = PhoneNumber;
        public string City { get; set; } = City;
        public string Zip { get; set; } = Zip;
        public string District { get; set; } = District;
        public string AddressLine { get; set; } = AddressLine;
        public string AddressTitle { get; set; } = AddressTitle;
    }
}