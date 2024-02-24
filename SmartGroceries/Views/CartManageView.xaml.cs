using SmartGroceries.Models;
using SmartGroceries.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Logique d'interaction pour CartManageView.xaml
    /// </summary>
    public partial class CartManageView : UserControl
    {
        public CartManageView()
        {
            InitializeComponent();

            IsCompactDisplay.IsChecked = (Application.Current as App).preferences.GlobalCompactMode;
            ChangeDisplayMode();
        }

        private void ShopCart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var shop = (ShopCart.SelectedItem as Shop);
            if (shop != null)
            {
                (DataContext as CartArticlesManageViewModel).Shop = shop;
            }
        }
        private void ChangeDisplayMode()
        {
            string key = "CartArticleTemplate";
            if (IsCompactDisplay.IsChecked == true)
                key = "CartArticleTemplateCompact";

            var template = TryFindResource(key);
            if (template != null)
                lbCartArticles.ItemTemplate = template as DataTemplate;
        }

        private void IsCompactDisplay_Checked(object sender, RoutedEventArgs e)
        {
            ChangeDisplayMode();
        }
    }
}
