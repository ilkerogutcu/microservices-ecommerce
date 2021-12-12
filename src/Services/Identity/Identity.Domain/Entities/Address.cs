using System;
using Identity.Domain.Common;

namespace Identity.Domain.Entities
{
    public class Address : BaseEntity, IEntity
    {
        public Guid CityId { get; set; }
        public City City { get; set; }
        public Guid DistrictId { get; set; }
        public District District { get; set; }
        public string Zip { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressLine { get; set; }
        public string AddressTitle { get; set; }
    }
}