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
    /// Logique d'interaction pour MakeShopWindow.xaml
    /// </summary>
    public partial class MakeShopWindow : Window
    {
        private ViewModels.CartViewModel _cartViewModel;
        public MakeShopWindow(CartViewModel cartViewModel)
        {
            InitializeComponent();
            _cartViewModel = cartViewModel;
            NameTextBox.Text = cartViewModel.ShopName;
        }

        private void Cancel_Button(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MakeShop_Button(object sender, RoutedEventArgs e)
        {
            Shop shop = GlobalDatabase.TryGetShop(NameTextBox.Text, GroupTextBox.Text, AddressTextBox.Text);
            if (shop == null)
            {
                shop = new Shop(NameTextBox.Text, GroupTextBox.Text, AddressTextBox.Text);
                if (GlobalDatabase.SaveShop(shop))
                    _cartViewModel.Shop = shop;
                Close();
            }
            else
                MessageBox.Show("At least a Shop with this Name already Exists\nRename or set a new Group and Address !", "Error : shop already Exists");
        }
    }
}
