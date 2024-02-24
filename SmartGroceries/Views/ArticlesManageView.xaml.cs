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
    /// Logique d'interaction pour ArticlesManageView.xaml
    /// </summary>
    public partial class ArticlesManageView : UserControl
    {
        public ArticlesManageView()
        {
            InitializeComponent();

            IsCompactDisplay.IsChecked = (Application.Current as App).preferences.GlobalCompactMode;
            ChangeDisplayMode();
        }

        private void ChangeDisplayMode()
        {
            string key = "ArticleTemplate";
            if (IsCompactDisplay.IsChecked == true)
                key = "ArticleTemplateCompact";

            var template = TryFindResource(key);
            if (template != null)
                lbArticles.ItemTemplate = template as DataTemplate;
        }

        private void IsCompactDisplay_Click(object sender, RoutedEventArgs e)
        {
            ChangeDisplayMode();
        }
    }
}
