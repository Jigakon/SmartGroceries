using SmartGroceries.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartGroceries.Views
{
    /// <summary>
    /// Logique d'interaction pour CompareShopArticlesView.xaml
    /// </summary>
    public partial class CompareShopArticlesView : UserControl
    {
        List<ComboBox> SelectorShops = new List<ComboBox>();
        List<Button> ArticlesReset = new List<Button>();
        List<ComboBox> SelectorArticles = new List<ComboBox>();

        public CompareShopArticlesView()
        {
            InitializeComponent();

            SelectorShops.Add(Selector0Shop);
            SelectorShops.Add(Selector1Shop);
            SelectorShops.Add(Selector2Shop);

            ArticlesReset.Add(DeleteArticleBtn0);
            ArticlesReset.Add(DeleteArticleBtn1);
            ArticlesReset.Add(DeleteArticleBtn2);

            SelectorArticles.Add(Selector0Article);
            SelectorArticles.Add(Selector1Article);
            SelectorArticles.Add(Selector2Article);
        }

        private void SelectorShop_SelectionChanged(int v)
        {
            if (v >= 3 && v < 0) return;
            SelectorArticles[v].IsEnabled = ArticlesReset[v].IsEnabled = SelectorShops[v].SelectedItem != null;
            if (SelectorShops[v].SelectedItem == null) return;
            var shop = (SelectorShops[v].SelectedItem as Models.Shop);
            SelectorArticles[v].ItemsSource = shop?.ShopArticles.Values;
            SelectorArticles[v].SelectedItem = null;
            var viewModel = (DataContext as ViewModels.CompareShopArticleViewModel);
            viewModel?.ClearPrices(v);
            viewModel?.ChangeName(v, $"Article | Brand | {shop?.Name ?? "Shop"}");
        }
        private void SelectorArticle_SelectionChanged(int v)
        {
            if (v >= 3 && v < 0 || SelectorArticles[v].SelectedItem == null) return;
            ShopArticle shopArticle = SelectorArticles[v].SelectedItem as Models.ShopArticle;
            var viewModel = (DataContext as ViewModels.CompareShopArticleViewModel);
            if (shopArticle == null || viewModel == null) return;
            viewModel?.ChangePrices(v, shopArticle?.ArticleInfos);
            viewModel?.ChangeName(v, shopArticle?.NameAndBrand + $" | {shopArticle.Shop.Name}");
            viewModel?.ChangeUnit(v, shopArticle?.ArticleUnit ?? Unit.Weight);
        }
        private void DeleteArticle(int v, string shopName = "Shop")
        {
            if (v >= 3 && v < 0) return;
            var viewModel = (DataContext as ViewModels.CompareShopArticleViewModel);
            SelectorArticles[v].Text = null;
            SelectorArticles[v].SelectedItem = null;
            viewModel?.ClearPrices(v);
            viewModel?.ChangeName(v, $"Article | Brand | {shopName}");
            viewModel?.ChangeUnit(v, Unit.Weight);
        }
        private void DeleteShop(int v)
        {
            DeleteArticle(v);
            SelectorShops[v].SelectedItem = null;
        }
        private void SelectorArticle_TextChanged(int v)
        {
            if (v >= 3 && v < 0) return;
            var shopArticles = (SelectorShops[v].SelectedItem as Models.Shop)?.ShopArticles;
            var SearchedShopArticles = new List<ShopArticle>();
            foreach (var shopArticle in shopArticles.Values)
            {
                bool textIsNull = string.IsNullOrEmpty(SelectorArticles[v].Text);
                if (textIsNull || (!textIsNull && shopArticle.Article.Name.ToLower().Contains(SelectorArticles[v].Text.ToLower())))
                    SearchedShopArticles.Add(shopArticle);
            }
            SelectorArticles[v].ItemsSource = SearchedShopArticles ?? shopArticles.Values.ToList();
        }

        private void Selector0Shop_SelectionChanged(object sender, SelectionChangedEventArgs e) => SelectorShop_SelectionChanged(0);
        private void Selector1Shop_SelectionChanged(object sender, SelectionChangedEventArgs e) => SelectorShop_SelectionChanged(1);
        private void Selector2Shop_SelectionChanged(object sender, SelectionChangedEventArgs e) => SelectorShop_SelectionChanged(2);
        private void Selector0Article_SelectionChanged(object sender, SelectionChangedEventArgs e) => SelectorArticle_SelectionChanged(0);
        private void Selector1Article_SelectionChanged(object sender, SelectionChangedEventArgs e) => SelectorArticle_SelectionChanged(1);
        private void Selector2Article_SelectionChanged(object sender, SelectionChangedEventArgs e) => SelectorArticle_SelectionChanged(2);
        private void DeleteArticleBtn0_Click(object sender, RoutedEventArgs e) => DeleteArticle(0, (SelectorShops[0].SelectedItem as Shop)?.Name);
        private void DeleteArticleBtn1_Click(object sender, RoutedEventArgs e) => DeleteArticle(1, (SelectorShops[1].SelectedItem as Shop)?.Name);
        private void DeleteArticleBtn2_Click(object sender, RoutedEventArgs e) => DeleteArticle(2, (SelectorShops[2].SelectedItem as Shop)?.Name);
        private void DeleteShopBtn0_Click(object sender, RoutedEventArgs e) => DeleteShop(0);
        private void DeleteShopBtn1_Click(object sender, RoutedEventArgs e) => DeleteShop(1);
        private void DeleteShopBtn2_Click(object sender, RoutedEventArgs e) => DeleteShop(2);
        private void Selector0Article_TextChanged(object sender, TextChangedEventArgs e) => SelectorArticle_TextChanged(0);
        private void Selector1Article_TextChanged(object sender, TextChangedEventArgs e) => SelectorArticle_TextChanged(1);
        private void Selector2Article_TextChanged(object sender, TextChangedEventArgs e) => SelectorArticle_TextChanged(2);
    }
}
