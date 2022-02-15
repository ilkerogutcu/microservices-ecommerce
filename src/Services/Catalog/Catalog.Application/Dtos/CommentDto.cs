using System;

namespace Catalog.Application.Dtos
{
    public class CommentDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}