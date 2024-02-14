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
        private readonly ShopManageViewModel _viewModel;
        private ShopArticle _shopArticle;

        public MakeShopArticleWindow(ShopManageViewModel shopManageViewModel)
        {
            InitializeComponent();

            _viewModel = shopManageViewModel;
            Shop shop = GlobalDatabase.GetShop(shopManageViewModel.ShopViewModel.Id);
            if (shop == null)
                shop = shopManageViewModel.MakeShop();
            _shopArticle = new ShopArticle(shop, new Article(), new List<ArticleInfo>());

            _ArticleUnit.SelectedItem = Unit.Weight;
            _shopArticle.ArticleUnit = Unit.Weight;
        }

        private void _ArticleName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var article = (_ArticleName.SelectedItem as Models.Article);
            if (article == null)
                return;
            _ArticleBrand.Text = article.Brand;
            _shopArticle.Article = article;
        }

        private void _ArticleUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Models.Unit? unit = (_ArticleUnit.SelectedItem as Models.Unit?);
            if (unit != null) 
                _shopArticle.ArticleUnit = (Models.Unit)unit;
        }

        private void _ArticleBrand_TextChanged(object sender, TextChangedEventArgs e)
        {
            _shopArticle.Article.Brand = _ArticleBrand.Text;
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            // Search if this new article already exists in the database, based on it's name and brand
            Article article = GlobalDatabase.GetArticle(_shopArticle.Article.Name, _shopArticle.Article.Brand);
            if (article != null)
                _shopArticle.Article = article;
            else
                _shopArticle.Article.Id = Guid.NewGuid();
            _viewModel.AddShopArticle(_shopArticle);
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void _ArticleName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _shopArticle.Article.Name = _ArticleName.Text;
        }
    }
}
