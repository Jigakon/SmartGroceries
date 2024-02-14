using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Models
{
    public class ArticleInfo
    {
        public DateTime Date { get; set; }
        public float Price { get; set; }
        /// <summary>
        /// in what unit quantity the Article is selled : weight (kg), volume (l) or piece.
        /// Examples : Water 1l, Chicken 1.5kg... 
        /// </summary>
        public float UnitQuantity { get; set; }

        public ArticleInfo()
        {
            Price = 0f;
            Date = DateTime.Today;
            UnitQuantity = 0;
        }

        public ArticleInfo(float price, DateTime date, float unit = 0)
        {
            Price = price;
            Date = date.Date;
            UnitQuantity = unit;
        }
    }
}
