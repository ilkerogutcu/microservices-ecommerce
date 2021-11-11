using System.Collections.Generic;
using Catalog.Domain.Common;

namespace Catalog.Domain.Entities
{
    public class Product : BaseEntity
    {
        public Category Category { get; set; }
        public Media ThumbnailMedia { get; set; }
        public Brand Brand { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string ModelCode { get; set; }
        public int ReviewsCount { get; set; }
        public double? RatingAverage { get; set; }
        public int RatingCount { get; set; }

        public IList<Media> Medias { get; set; } = new List<Media>();
        public IList<Comment> Comments { get; set; } = new List<Comment>();
        public IList<Sku> Skus { get; set; } = new List<Sku>();


        public void AddMedia(Media media)
        {
            Medias.Add(media);
        }

        public void AddComment(Comment comment)
        {
            Comments.Add(comment);
        }

        public void AddSku(Sku sku)
        {
            Skus.Add(sku);
        }
    }
}