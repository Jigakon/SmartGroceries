using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SmartGroceries.Models
{
    public class ArticleDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public Guid TagID { get; set; }

        //used for JsonSerializer
        public ArticleDTO() 
        {
            Id = Guid.Empty; 
            Name = "Debug::ArticleDTO::Name";
            Brand = "Debug::ArticleDTO::Brand";
            TagID = Guid.Empty;
        }

        public ArticleDTO(Article article) 
        {
            Id = article.Id;
            Name = article.Name;
            Brand = article.Brand;
            TagID = article.Tag?.Id ?? Guid.Empty;
        }

        public Article MakeArticle()
        {
            var article = GlobalDatabase.TryGetArticle(Id);
            if (article == null) 
            {
                Tag tag = TagID != Guid.Empty ? GlobalDatabase.TryGetTag(TagID) : null;
                article = new Article(Id, Name, Brand, tag);
            }
            return article;
        }
    }
}
