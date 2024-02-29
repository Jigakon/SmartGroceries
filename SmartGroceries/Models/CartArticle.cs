using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Models
{
    public class CartArticle
    {
        public Cart Cart { get; set; }
        public Article Article { get; set; }
        public uint Quantity { get; set; }
        public float Price { get; set; }
        public float UnitQuantity { get; set; }


        public CartArticle(Cart cart, Article article = null, uint quantity = 1, float price = 0f, float unitQuantity = -1f) 
        {
            Cart = cart;
            Article = article ?? new Article();
            Quantity = quantity;
            Price = price;
            UnitQuantity = unitQuantity;
        }
    }
}
