using SmartGroceries.Commands;
using SmartGroceries.Models;
using SmartGroceries.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartGroceries.ViewModels
{
    public class ShopArticleViewModel : ViewModelBase
    {
        private readonly Stores.NavigationStore _navigationStore;

        public bool IsNew = false;
        public Article Article { get; set; }
        public Shop Shop { get; set; }
        public List<ArticleInfo> ArticleInfos { get; set; }
        public Unit ArticleUnit { get; set; }
        public float UnitQuantity { get; set; }

        private Unit unit { get; }
        public string UnitText
        {
            get
            {
                switch(unit)
                {
                    case Unit.Piece: return "Piece";
                    case Unit.Weight: return "kg";
                    case Unit.Volume: return "l";
                    default: return "";
                }
            }
        }

        public ICommand GoToArticleInfoManageCommand { get; }
        public ICommand GoToShopManageCommand { get; }

        public ArticleInfo lastShopArticle => ArticleInfos.Last();
        public string lastArticlePriceString { get => ArticleInfos.Count != 0 ? lastShopArticle.Price.ToString() : "-1"; }

        public ShopArticleViewModel(ShopArticle shopArticle, Stores.NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            Article = shopArticle.Article;
            Shop = shopArticle.Shop;

            unit = shopArticle.ArticleUnit;

            ArticleUnit = shopArticle.ArticleUnit;
            UnitQuantity = shopArticle.UnitQuantity;

            ArticleInfos = new List<ArticleInfo>();
            foreach(var articleInfo in shopArticle.ArticleInfos) 
                ArticleInfos.Add(articleInfo);

            GoToArticleInfoManageCommand = new Commands.NavigateCommand(new Services.NavigationService(_navigationStore, CreateArticleInfosManageViewModel));
        }

        public ArticleInfosManageViewModel CreateArticleInfosManageViewModel()
        {
            return new ArticleInfosManageViewModel(this, new Services.NavigationService(_navigationStore, CreateShopManageViewModel));
        }

        public ShopManageViewModel CreateShopManageViewModel() 
        {
            var shopViewModel = new ShopViewModel(Shop, _navigationStore);
            return new ShopManageViewModel(shopViewModel, new Services.NavigationService(_navigationStore, shopViewModel.CreateShopsManageViewModel));
        }
    }
}
