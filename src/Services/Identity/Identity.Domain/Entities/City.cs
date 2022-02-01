using System.Collections.Generic;
using Identity.Domain.Common;

namespace Identity.Domain.Entities
{
    public class City : BaseEntity, IEntity
    {
        public string Name { get; set; }
        public List<District> Districts { get; set; } = new List<District>();

        public City(string name,string createdBy)
        {
            Name = name;
            CreatedDate = System.DateTime.Now;
            CreatedBy = createdBy;
        }
        public City()
        {

        }
        public void AddDistrict(District district)
        {
            Districts.Add(district);
        }
    }
}