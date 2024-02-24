using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Logique d'interaction pour ChangePathsWindow.xaml
    /// </summary>
    public partial class ChangePathsWindow : Window
    {


        public ChangePathsWindow()
        {
            InitializeComponent();
            var pref = (Application.Current as App).preferences;
            TagsPath_TextBox.Text = pref.TagsPath;
            ArticlesPath_TextBox.Text = pref.ArticlesPath;
            ShopsPath_TextBox.Text = pref.ShopsPath;
            CartsPath_TextBox.Text = pref.CartsPath;
        }

        private void OpenDialog(TextBox textBox)
        {
            FileDialog dialog = new OpenFileDialog();
            dialog.FileName = "Document"; // Default file name
            dialog.DefaultExt = ".json"; // Default file extension
            dialog.Filter = "JSON files (.json)|*.json|All files|*.*"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                textBox.Text = dialog.FileName;
            }
        }

        private void TagOpenDialog_Click(object sender, RoutedEventArgs e) => OpenDialog(TagsPath_TextBox);
        private void ArticleOpenDialog_Click(object sender, RoutedEventArgs e) => OpenDialog(ArticlesPath_TextBox);
        private void ShopOpenDialog_Click(object sender, RoutedEventArgs e) => OpenDialog(ShopsPath_TextBox);
        private void CartOpenDialog_Click(object sender, RoutedEventArgs e) => OpenDialog(CartsPath_TextBox);

        private bool IsFileAJSON(string path) => System.IO.Path.GetExtension(path) == ".json";
        
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            var pref = (Application.Current as App).preferences;
            string tagPath = TagsPath_TextBox.Text;
            string articlePath = ArticlesPath_TextBox.Text;
            string shopPath = ShopsPath_TextBox.Text;
            string cartPath = CartsPath_TextBox.Text; 
            if (File.Exists(tagPath) && IsFileAJSON(tagPath)) pref.TagsPath = tagPath;
            if (File.Exists(articlePath) && IsFileAJSON(articlePath)) pref.ArticlesPath = articlePath;
            if (File.Exists(shopPath) && IsFileAJSON(shopPath)) pref.ShopsPath = shopPath;
            if (File.Exists(cartPath) && IsFileAJSON(cartPath)) pref.CartsPath = cartPath;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
