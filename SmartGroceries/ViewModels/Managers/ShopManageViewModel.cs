using SmartGroceries.Models;
using SmartGroceries.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace SmartGroceries.ViewModels
{
    public class ShopManageViewModel : ViewModelBase
    {
        private readonly Stores.NavigationStore _navigationStore;

        public ShopViewModel ShopViewModel { get; set; }

        public string Name => ShopViewModel.Name;

        public ObservableCollection<ShopArticleViewModel> shopArticles { get; set; }

        public ObservableCollection<ShopArticleViewModel> SearchedViewModels { get => shopArticles; }

        public ICommand GoToManageShopsCommand { get; }
        public ICommand AddShopArticleCommand { get; }
        public ICommand SaveArticlesCommand { get; }
        public ShopManageViewModel(ShopViewModel shopViewModel, Services.NavigationService ShopsManageViewService)
        {
            _navigationStore = ShopsManageViewService.GetNavigationStore();

            ShopViewModel = shopViewModel;
            shopArticles = new ObservableCollection<ShopArticleViewModel>();
            foreach (var shopArticle in shopViewModel.Articles)
                shopArticles.Add(shopArticle);

            GoToManageShopsCommand = new Commands.NavigateCommand(ShopsManageViewService);
            AddShopArticleCommand = new Commands.MakeShopArticleCommand(this);
            SaveArticlesCommand = new Commands.SaveShopsCommand(this);
        }

        public void AddShopArticle(ShopArticle shopArticle)
        {
            if (shopArticles.FirstOrDefault(x => x.Article.Id == shopArticle.Article.Id) == null)
                shopArticles.Add(new ShopArticleViewModel(shopArticle, _navigationStore));
            Save();
        }

        public Shop MakeShop()
        {
            Shop shop = GlobalDatabase.GetShop(ShopViewModel.Id);
            if (shop == null)
            {
                shop = new Shop(ShopViewModel.Id, ShopViewModel.Name, ShopViewModel.Group, ShopViewModel.Location);
                foreach (var SAVM in shopArticles)
                {
                    List<ArticleInfo> articleInfos = new List<ArticleInfo>();
                    foreach (var articleInfo in SAVM.ArticleInfos)
                        articleInfos.Add(new ArticleInfo(articleInfo.Price, articleInfo.Date, articleInfo.UnitQuantity));
                    shop.TryAddArticle(SAVM.Article, SAVM.ArticleUnit, SAVM.UnitQuantity, articleInfos);
                }
            }
            return shop;
        }

        public void Save()
        {
            Shop shop = MakeShop();

            foreach (var shopArticle in shopArticles)
                GlobalDatabase.SaveArticle(shopArticle.Article);

            GlobalDatabase.SaveShop(shop);
        }
    }
}
