using System;
using MongoDB.Bson;

namespace Catalog.Domain.Entities
{
    public class Comment 
    {
        public string Id { get; } = ObjectId.GenerateNewId().ToString();
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedDate { get; } = DateTime.Now;
        public string CreatedBy { get; set; }

    }
}