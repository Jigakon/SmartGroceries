using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Models
{
    public class ShopArticle
    {
        public Shop Shop { get; set; }
        public Article Article { get; set; }
        private List<ArticleInfo> _articleInfos { get; set; }
        public List<ArticleInfo> ArticleInfos => _articleInfos;
        public Unit ArticleUnit { get; set; }
        public float UnitQuantity { get; set; }

        public ShopArticle(Shop shop, Article article, List<ArticleInfo> infos = null, Unit unit = Unit.Weight, float unityQuantity = -1f) 
        {
            Shop = shop;
            Article = article;
            _articleInfos = infos ?? new List<ArticleInfo>();
            ArticleUnit = unit;
            UnitQuantity = unityQuantity;
        }

        public ArticleInfo GetArticleInfo(DateTime date)
        {
            return _articleInfos.FirstOrDefault(x => x.Date.Date == date.Date);
        }

        public void AddArticleInfo(ArticleInfo articleInfo, bool Override = true)
        {
            ArticleInfo searchedArticleInfo = GetArticleInfo(articleInfo.Date);
            if (searchedArticleInfo == null)
                _articleInfos.Add(articleInfo);
            else if (Override)
            {
                searchedArticleInfo.Price = articleInfo.Price;
                searchedArticleInfo.UnitQuantity = articleInfo.UnitQuantity;
            }
        }

        public void ClearArticleInfos()
        {
            _articleInfos.Clear();
        }
    }
}
