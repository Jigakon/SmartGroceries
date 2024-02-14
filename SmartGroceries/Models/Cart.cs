using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Models
{
    public class Cart
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public Shop Shop { get; set; }
        public List<CartArticle> CartArticles { get; set; }

        public Cart(Guid id, string name, DateTime date, Shop shop, List<CartArticle> cartArticles = null)
        {
            Id = id;
            Name = name;
            Date = date.Date;
            Shop = shop;
            CartArticles = cartArticles ?? new List<CartArticle>();
        }

        public Cart()
        {
            Id = Guid.NewGuid();
            Name = "New Cart";
            Date = DateTime.Today;
            Shop = null;
            CartArticles = new List<CartArticle>();
        }

        public void AddCartArticle(CartArticle article)
        {
            CartArticles.Add(article);
        }
    }
}
