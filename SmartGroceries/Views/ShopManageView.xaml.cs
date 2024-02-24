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
    /// Logique d'interaction pour ShopManageView.xaml
    /// </summary>
    public partial class ShopManageView : UserControl
    {
        public ShopManageView()
        {
            InitializeComponent();
            IsCompactDisplay.IsChecked = (Application.Current as App).preferences.GlobalCompactMode;
            ChangeDisplayMode();
        }

        private void ChangeDisplayMode()
        {
            string key = "ShopArticleTemplate";
            if (IsCompactDisplay.IsChecked == true)
                key = "ShopArticleTemplateCompact";

            var template = TryFindResource(key);
            if (template != null)
                lbShopArticles.ItemTemplate = template as DataTemplate;
        }

        private void IsCompactDisplay_Checked(object sender, RoutedEventArgs e)
        {
            ChangeDisplayMode();
        }
    }
}
