using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Models
{
    internal class CartDTO
    {
        public Guid Id { get; set; }
        public Guid ShopID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public List<CartArticleDTO> ArticleIds { get; set; }

        public CartDTO()
        {
            Id = Guid.Empty;
            ShopID = Guid.Empty;
            Name = "New Cart";
            Date = DateTime.Today;
            ArticleIds = new List<CartArticleDTO>();
        }

        public CartDTO(Cart cart) 
        {
            Id = cart.Id;
            if (cart.Shop != null)
                ShopID = cart.Shop.Id;
            else
                ShopID = Guid.Empty;
            Name = cart.Name;
            Date = cart.Date;
            ArticleIds = new List<CartArticleDTO>();
            foreach(var cartArticle in cart.CartArticles)
                ArticleIds.Add(new CartArticleDTO(cartArticle));
        }

        public Cart MakeCart()
        {
            Shop shop = GlobalDatabase.TryGetShop(ShopID);
            Cart cart = new Cart(Id, Name, Date, shop);
            foreach(var cartArticle in ArticleIds)
            {
                Article article = GlobalDatabase.TryGetArticle(cartArticle.ArticleID);
                if(article != null) cart.CartArticles.Add(new CartArticle(cart, article, cartArticle.Quantity));
            }
            return cart;
        }
    }
}
