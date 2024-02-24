using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using SmartGroceries.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace SmartGroceries.ViewModels
{
    internal class CartInfomationsViewModel : ViewModelBase
    {
        public Cart _myCart;
        public Cart myCart
        {
            get => _myCart;
            set
            {
                _myCart = value;
                OnPropertyChanged(nameof(myCart));
                RefreshPie();
            }
        }

        private CartViewModel _cart;
        public CartViewModel Cart
        {
            get => _cart;
            set
            {
                _cart = value;
                OnPropertyChanged(nameof(Cart));
                SortedCartArticles = new ObservableCollection<CartArticleViewModel>(Cart.CartArticles.OrderBy(x => x.Tag?.Id ?? Guid.Empty));
            }
        }

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

        public void RefreshPie()
        {
            Prices.Clear();

            Dictionary<Guid, float> prices = new Dictionary<Guid, float>();

            Cart = new CartViewModel(myCart, this, null);
            float TotalPrice = 0f;
            foreach (var cartArticle in myCart.CartArticles)
            {
                cartArticle.Price = myCart.Shop.GetArticleInfo(cartArticle.Article.Id, myCart.Date).Price;
                float quantityPrice = cartArticle.Price * cartArticle.Quantity;
                Guid id = cartArticle.Article.Tag?.Id ?? Guid.Empty;
                if (prices.ContainsKey(id))
                    prices[id] += quantityPrice;
                else
                    prices[id] = quantityPrice;
                TotalPrice += quantityPrice;
            }
            foreach (var itemPair in prices)
            {

                Tag tag = GlobalDatabase.TryGetTag(itemPair.Key);
                string tagName = itemPair.Key != Guid.Empty ? tag.Name : "No Tag";
                Color tagColor = (Color)ColorConverter.ConvertFromString(tag?.Color ?? "#FFFFFF");
                PieSeries serie = new PieSeries
                {
                    Values = new ChartValues<float> { itemPair.Value },
                    Title = tagName,
                    LabelPoint = point => point.Y.ToString("0.00") + " €",
                    Fill = new SolidColorBrush(tagColor),
                    DataLabels = true,
                    Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
                    FontSize = 16,
                };
                Prices.Add(serie);
            }
            Prices = Prices.OrderBy(x => x.Title).AsSeriesCollection();
            
        }

        private ObservableCollection<CartArticleViewModel> _sortedCartArticles;
        public ObservableCollection<CartArticleViewModel> SortedCartArticles
        {
            get => _sortedCartArticles;
            set
            {
                _sortedCartArticles = value;
                OnPropertyChanged(nameof(SortedCartArticles));
            }
        }

        public CartInfomationsViewModel()
        {
            SortedCartArticles = new ObservableCollection<CartArticleViewModel>();

            Prices = new SeriesCollection{
                new PieSeries
                {
                    Values = new ChartValues<float> { 1 },
                    Title = "empty",
                    LabelPoint = point => point.Y.ToString("0.00") + " €",
                    Fill = new SolidColorBrush(Color.FromRgb(127,127,127)),
                    Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
                    FontSize = 16,
                }
            };
        }
    }
}
