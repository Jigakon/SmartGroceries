using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Models
{
    public class ArticleDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public List<Guid> TagIDs { get; set; }

        public ArticleDTO() 
        {
            Id = Guid.Empty; 
            Name = "Debug::ArticleDTO::Name";
            Brand = "Debug::ArticleDTO::Brand";
            TagIDs = new List<Guid>();
        }

        public ArticleDTO(Article article) 
        {
            Id = article.Id;
            Name = article.Name;
            Brand = article.Brand;
            TagIDs = new List<Guid>();
            foreach (var item in article.Tags)
                TagIDs.Add(item.Id);
        }

        public Article MakeArticle()
        {
            var article = GlobalDatabase.GetArticle(Id);
            if (article == null) 
            {
                article = new Article(Id, Name, Brand);
                foreach(var tagID in TagIDs)
                    article.Tags.Add(GlobalDatabase.TryGetTag(tagID));
            }
            return article;
        }
    }
}
