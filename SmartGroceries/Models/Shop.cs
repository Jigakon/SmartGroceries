using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartGroceries.Models
{
    public class Shop
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public string Location { get; set; }
        public List<ShopArticle> ShopArticles { get; set; }

        public Shop(Guid id, string name, string group, string location, List<ShopArticle> shopArticles = null)
        {
            Id = id;
            Name = name;
            Group = group;
            Location = location;
            ShopArticles = shopArticles ?? new List<ShopArticle>();
        }

        public Shop(string name = "Shop Name", string group = "Shop Group", string location = "Random Address", List<ShopArticle> shopArticles = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Group = group;
            Location = location;
            ShopArticles = shopArticles ?? new List<ShopArticle>();
        }

        public void TryAddArticle(ShopArticle article)
        {
            if (article.Article != null)
            {
                if (GetShopArticle(article.Article.Id) == null)
                {
                    article.Shop = this;
                    ShopArticles.Add(article);
                }
            }
            else
                MessageBox.Show("An article registered in shop is doesn't exists in Article Database");
        }

        public void TryAddArticle(Article article, Unit unit = Unit.Weight, float unitQuantity = -1f, List<ArticleInfo> infos = null)
        {
            if (GetShopArticle(article.Id) == null)
                ShopArticles.Add(new ShopArticle(this, article, infos, unit, unitQuantity));
        }

        public ShopArticle GetShopArticle(Guid articleID)
        {
            return ShopArticles.FirstOrDefault(x => x.Article.Id == articleID);
        }

        public ArticleInfo GetArticleInfo(Guid articleID, DateTime date)
        {
            return GetShopArticle(articleID)?.GetArticleInfo(date);
        }

        public void AddArticleInfo(Guid id, ArticleInfo articleInfo)
        {
            var shopArticle = GetShopArticle(id);
            shopArticle.AddArticleInfo(articleInfo);
        }
    }
}
