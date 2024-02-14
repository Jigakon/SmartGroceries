﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmartGroceries.Models
{
    public class ShopArticleDTO
    {
        public Guid ArticleID { get; set; }
        public List<ArticleInfo> ArticleInfos { get; set; }
        public Unit ArticleUnit { get; set; }
        public float UnitQuantity { get; set; }

        public ShopArticleDTO()
        {
            ArticleID = Guid.Empty;
            ArticleInfos = new List<ArticleInfo>();
            ArticleUnit = Unit.Weight;
            UnitQuantity = -1f;
        }

        public ShopArticleDTO(ShopArticle shopArticle)
        {
            ArticleID = shopArticle.Article.Id;
            ArticleInfos = shopArticle.ArticleInfos;
            ArticleUnit = shopArticle.ArticleUnit;
            UnitQuantity = shopArticle.UnitQuantity;
        }

        public ShopArticle MakeShopArticle(Shop shop)
        {
            Article article = GlobalDatabase.GetArticle(ArticleID);
            return new ShopArticle(shop, article, ArticleInfos, ArticleUnit, UnitQuantity);
        }
    }
}
