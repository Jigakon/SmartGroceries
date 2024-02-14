using SmartGroceries.Commands;
using SmartGroceries.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SmartGroceries.ViewModels
{
    public class ArticleInfosManageViewModel : ViewModelBase
    {
        private ShopArticleViewModel _shopArticleViewModel;
        public ShopArticleViewModel shopArticleViewModel => _shopArticleViewModel;

        public ObservableCollection<ArticleInfoViewModel> ArticleInfos { get; set; }
        public ObservableCollection<ArticleInfoViewModel> SearchedViewModels => ArticleInfos;

        public ICommand MakeArticleInfoCommand { get; }
        public ICommand SaveArticleInfosCommand { get; }
        public ICommand GoToManageShopCommand { get; }

        public ArticleInfosManageViewModel(ShopArticleViewModel shopArticleViewModel, Services.NavigationService ShopManageViewService) 
        {
            _shopArticleViewModel = shopArticleViewModel;

            ArticleInfos = new ObservableCollection<ArticleInfoViewModel>();

            MakeArticleInfoCommand = new Commands.MakeArticleInfoCommand(this);
            SaveArticleInfosCommand = new Commands.SaveArticleInfosCommand(this);
            GoToManageShopCommand = new Commands.NavigateCommand(ShopManageViewService);
            ResetArticleInfos();
        }       

        public void Sort()
        {
            ArticleInfos = new ObservableCollection<ArticleInfoViewModel>(ArticleInfos.OrderBy(x => x.Date));
            OnPropertyChanged(nameof(ArticleInfos));
            OnPropertyChanged(nameof(SearchedViewModels));
        }

        public void ResetArticleInfos()
        {
            ArticleInfos.Clear();
            foreach (var item in _shopArticleViewModel.ArticleInfos)
                ArticleInfos.Add(new ArticleInfoViewModel(item, this));
            Sort();
        }

        public void AddArticleInfo(ArticleInfoViewModel articleInfo)
        {
            ArticleInfos.Add(articleInfo);
            Sort();
        }

        public void RemoveArticleInfo(ArticleInfoViewModel articleInfo)
        {
            ArticleInfos.Remove(articleInfo);
            OnPropertyChanged(nameof(ArticleInfos));
        }

        public void Save()
        {
            Shop searchedShop = GlobalDatabase.GetShop(_shopArticleViewModel.Shop.Id);
            if (searchedShop != null)
            {
                ShopArticle searchedShopArticle = searchedShop.GetShopArticle(_shopArticleViewModel.Article.Id);
                if (searchedShopArticle != null )
                {
                    searchedShopArticle.ClearArticleInfos();
                    foreach (ArticleInfoViewModel articleInfo in ArticleInfos)
                        searchedShopArticle.AddArticleInfo(articleInfo.MakeArticleInfo());
                }
            }
        }
    }
}
