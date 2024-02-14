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
    /// Logique d'interaction pour CartsManageView.xaml
    /// </summary>
    public partial class CartsManageView : UserControl
    {
        public CartsManageView()
        {
            InitializeComponent();
        }

        private void IsCompactDisplay_Checked(object sender, RoutedEventArgs e)
        {
            string key = "CartTemplate";
            if (IsCompactDisplay.IsChecked == true)
                key = "CartTemplateCompact";

            var template = TryFindResource(key);
            if (template != null)
                lbCarts.ItemTemplate = template as DataTemplate;
        }
    }
}
