using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Domain.Entities
{
    public class Comment 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } 
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedDate { get; set; } 
        public string CreatedBy { get; set; }
    }
}