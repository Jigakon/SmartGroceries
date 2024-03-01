using Microsoft.Win32;
using SmartGroceries.Models;
using SmartGroceries.Windows;
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

namespace SmartGroceries
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ChangePaths_Button(object sender, RoutedEventArgs e)
        {
            new ChangePathsWindow().ShowDialog();
            var pref = (Application.Current as App).preferences;
            if (GlobalDatabase.SetTagPath(pref.TagsPath))
                GlobalDatabase.LoadTagsFromJson();
            if (GlobalDatabase.SetArticlePath(pref.ArticlesPath))
                GlobalDatabase.LoadArticlesFromJson();
            if(GlobalDatabase.SetShopPath(pref.ShopsPath))
                GlobalDatabase.LoadShopsFromJson();
            if(GlobalDatabase.SetCartPath(pref.CartsPath))
                GlobalDatabase.LoadCartsFromJson();

            pref.Save();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
