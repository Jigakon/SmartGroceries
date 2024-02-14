using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Models
{
    public class CartArticleDTO
    {
        public Guid ArticleID { get; set; }
        public uint Quantity { get; set; }

        public CartArticleDTO()
        {
            ArticleID = Guid.Empty;
            Quantity = 0;
        }

        public CartArticleDTO(CartArticle cartArticle)
        {
            ArticleID = cartArticle.Article.Id;
            Quantity = cartArticle.Quantity;
        }

        public CartArticle MakeCartArticle(Cart cart)
        {
            Article article = GlobalDatabase.GetArticle(ArticleID);
            return new CartArticle(cart, article, Quantity);
        }
    }
}
