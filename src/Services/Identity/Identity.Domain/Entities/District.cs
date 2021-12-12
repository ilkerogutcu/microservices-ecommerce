using System;
using Identity.Domain.Common;

namespace Identity.Domain.Entities
{
    public class District : BaseEntity, IEntity
    {
        public Guid CityId { get; set; }
        public City City { get; set; }
        public string Name { get; set; }

        public District(City city, string name,string createdBy)
        {
            City = city;
            Name = name;
            CreatedDate = DateTime.Now;
            CreatedBy = createdBy;
        }
        public District()
        {

        }
    }
}