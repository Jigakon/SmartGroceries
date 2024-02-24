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
        public string NameAndBrand => $"{Article?.Name ?? "Article"} | {Article?.Brand ?? "Brand"}";
        public bool IsUnitFixed {  get; set; }
        public ShopArticle(Shop shop, Article article, List<ArticleInfo> infos = null, Unit unit = Unit.Weight, float unityQuantity = -1f, bool isUnitFixed = false)
        {
            Shop = shop;
            Article = article;
            _articleInfos = infos ?? new List<ArticleInfo>();
            ArticleUnit = unit;
            UnitQuantity = unityQuantity;
            IsUnitFixed = isUnitFixed;
        }

        public ArticleInfo GetArticleInfo(DateTime date)
        {
            return _articleInfos.FirstOrDefault(x => x.Date.Date == date.Date);
        }

        public ArticleInfo GetClosestArticleInfo(DateTime date)
        {
            switch (_articleInfos.Count)
            {
                case 0: return null;
                case 1: return ArticleInfos[0];
                default:
                    int closestIndex = 0;
                    double nbDays = (date - _articleInfos[0].Date).TotalDays;
                    for (int i = 1; i < _articleInfos.Count; i++)
                    {
                        double tempTotalDays = (date - _articleInfos[i].Date).TotalDays;
                        if (tempTotalDays > nbDays)
                        {
                            break;
                        }
                        else if (tempTotalDays < nbDays)
                        {
                            nbDays = tempTotalDays;
                            closestIndex = i;
                        }
                    }
                    return _articleInfos[closestIndex];
            }
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
