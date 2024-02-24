using LiveCharts;
using LiveCharts.Definitions.Series;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using SmartGroceries.Commands;
using SmartGroceries.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SmartGroceries.ViewModels
{
    public class ArticleInfosManageViewModel : ViewModelBase, ViewModelManage
    {
        private ShopArticleViewModel _shopArticleViewModel;
        public ShopArticleViewModel shopArticleViewModel => _shopArticleViewModel;

        public ObservableCollection<ArticleInfoViewModel> ArticleInfos { get; set; }
        public ObservableCollection<ArticleInfoViewModel> SearchedViewModels => ArticleInfos;

        #region Chart
        public Func<double, string> FormatterDate { get; set; }
        public Func<double, string> FormatterPrice { get; set; }
        public SeriesCollection _prices;
        public SeriesCollection Prices
        {
            get => _prices;
            set
            {
                _prices = value;
                OnPropertyChanged(nameof(Prices));
            }
        }
        public LineSeries CreateLineSerie(List<ArticleInfoViewModel> datas)
        {
            return new LineSeries
            {
                Values = new ChartValues<ArticleInfoViewModel>(datas),
                Title = "Prices",
                LabelPoint = point => point.Y.ToString("0.00") + " €",
                Fill = Brushes.Transparent,
            };
        }
        #endregion

        #region Commands
        public ICommand MakeArticleInfoCommand { get; }
        public ICommand SaveArticleInfosCommand { get; }
        public ICommand GoToManageShopCommand { get; }
        #endregion

        public string ShopName => _shopArticleViewModel.Shop.Name;
        public string ArticleName => _shopArticleViewModel.Article.Name;

        public ArticleInfosManageViewModel(ShopArticleViewModel shopArticleViewModel, Services.NavigationService ShopManageViewService)
        {
            _shopArticleViewModel = shopArticleViewModel;

            ArticleInfos = new ObservableCollection<ArticleInfoViewModel>();

            MakeArticleInfoCommand = new Commands.MakeArticleInfoCommand(this);
            SaveArticleInfosCommand = new Commands.SaveManageCommand(this);
            GoToManageShopCommand = new Commands.NavigateCommand(ShopManageViewService);

            FormatterDate = value => new System.DateTime((long)(value * TimeSpan.FromHours(1).Ticks)).ToString("yyyy-MM-dd");

            var dayConfig = LiveCharts.Configurations.Mappers.Xy<ArticleInfoViewModel>()
               .X(dayModel => (double)dayModel.Date.Ticks / TimeSpan.FromHours(1).Ticks)
               .Y(dayModel => dayModel.Price);

            ResetArticleInfos();

            Prices = new SeriesCollection(dayConfig)
            {
                CreateLineSerie(ArticleInfos.ToList()),
            };
        }

        public void Sort()
        {
            ArticleInfos = new ObservableCollection<ArticleInfoViewModel>(ArticleInfos.OrderBy(x => x.Date));
            OnPropertyChanged(nameof(ArticleInfos));
            OnPropertyChanged(nameof(SearchedViewModels));
            if (Prices != null && Prices.Count > 0 && Prices[0] != null)
            {
                Prices[0].Values.Clear();
                Prices[0].Values.AddRange(ArticleInfos);
            }
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

        public void Remove(ViewModelData data)
        {
            var articleInfo = data as ArticleInfoViewModel;
            ArticleInfos.Remove(articleInfo);
            OnPropertyChanged(nameof(ArticleInfos));
        }

        public void Save()
        {
            Shop searchedShop = GlobalDatabase.TryGetShop(_shopArticleViewModel.Shop.Id);
            if (searchedShop != null)
            {
                ShopArticle searchedShopArticle = searchedShop.GetShopArticle(_shopArticleViewModel.Article.Id);
                if (searchedShopArticle != null)
                {
                    searchedShopArticle.ClearArticleInfos();
                    foreach (ArticleInfoViewModel articleInfo in ArticleInfos)
                        searchedShopArticle.AddArticleInfo(articleInfo.MakeArticleInfo());
                }
            }
        }
    }
}
