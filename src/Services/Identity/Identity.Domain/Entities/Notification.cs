using System;
using Identity.Domain.Common;

namespace Identity.Domain.Entities
{
    public class Notification : BaseEntity, IEntity
    {
        public string RecipientUserId { get; set; }
        public User RecipientUser { get; set; }
        public string Content { get; set; }
        public bool Read { get; set; }
        public string Url { get; set; }
    }
}