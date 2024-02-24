using SmartGroceries.Models;
using SmartGroceries.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SmartGroceries.Windows
{
    /// <summary>
    /// Logique d'interaction pour MakeShopArticleWindow.xaml
    /// </summary>
    public partial class MakeShopArticleWindow : Window
    {
        private readonly ShopArticlesManageViewModel _viewModel;
        private Shop _shop;
        public MakeShopArticleWindow(ShopArticlesManageViewModel shopManageViewModel)
        {
            InitializeComponent();

            _viewModel = shopManageViewModel;
            _shop = GlobalDatabase.TryGetShop(shopManageViewModel.Id) ?? shopManageViewModel.MakeShop();
            
            _ArticleUnit.SelectedItem = Unit.Weight;
        }

        private void _ArticleName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var article = (_ArticleName.SelectedItem as Models.Article);
            if (article != null)
                _ArticleBrand.Text = article.Brand;
        }

        private void _ArticleUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Models.Unit? unit = (_ArticleUnit.SelectedItem as Models.Unit?);
        }

        private void _ArticleBrand_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_ArticleName.Text))
            {
            // Search if this new article already exists in the database, based on it's name and brand
                Article article = GlobalDatabase.TryGetArticle(_ArticleName.Text, _ArticleBrand.Text);
                if (article == null)
                    article = new Article(_ArticleName.Text, _ArticleBrand.Text);

                ShopArticle shopArticle = _shop.GetShopArticle(article.Id);
                if (shopArticle == null)
                {
                    Unit unit = _ArticleUnit.SelectedItem as Unit? ?? Unit.Weight;
                    shopArticle = new ShopArticle(_shop, article, new List<ArticleInfo>(), unit);
                    _viewModel.AddShopArticle(shopArticle);
                }
            }
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
