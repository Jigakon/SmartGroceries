using LiveCharts.Wpf;
using LiveCharts.Wpf.Points;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartGroceries.Views
{
    /// <summary>
    /// Logique d'interaction pour CartInformationsView.xaml
    /// </summary>
    public partial class CartInformationsView : UserControl
    {
        public CartInformationsView()
        {
            InitializeComponent();
        }

        private void CartSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as CartInfomationsViewModel).myCart = CartSelector.SelectedItem as Models.Cart;
        }
    }
}
