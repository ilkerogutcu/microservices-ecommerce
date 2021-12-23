using System;
using System.Collections.Generic;
using Identity.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Entities
{
    public class User : IdentityUser, IEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public string LastLoginIp { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime BirthDate { get; set; }
        public List<Address> Addresses { get; set; } = new List<Address>();
        public List<Notification> Notifications { get; set; } = new List<Notification>();

        public override string ToString()
        {
            return Id;
        }
    }
}