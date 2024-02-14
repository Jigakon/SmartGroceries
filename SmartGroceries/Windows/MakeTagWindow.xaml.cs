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
    /// Logique d'interaction pour MakeTagWindow.xaml
    /// </summary>
    public partial class MakeTagWindow : Window
    {
        private TagViewModel _tagViewModel;

        public MakeTagWindow(ViewModelBase containerViewModel)
        {
            InitializeComponent();

            _tagViewModel = new TagViewModel(new Tag("MyNewTag", "#FF0000FF"));
            _tagViewModel.ViewModelContainer = containerViewModel;
        }

        private void AddTagButton_Click(object sender, RoutedEventArgs e)
        {
            _tagViewModel.Color = _ColorPicker.SelectedColor.ToString();
            _tagViewModel.Name = _TagLabel.Text;

            var tagSearch = GlobalDatabase.TryGetTag(_tagViewModel.Name, _tagViewModel.Color);
            // if the id of the new tag already exists
            if (tagSearch != null)
            {
                _tagViewModel.Id = tagSearch.Id;
                _tagViewModel.Name = tagSearch.Name;
                _tagViewModel.Color = tagSearch.Color;
            }
            else
            {
                // temporary : save tags when we save Articles
                // ignore new tags if we don't save ?
                GlobalDatabase.TryAddTag(new Tag(_tagViewModel.Id, _tagViewModel.Name, _tagViewModel.Color));
                GlobalDatabase.SaveTagsInJSON();
            }
            (_tagViewModel.ViewModelContainer as ArticleViewModel).AddTag(_tagViewModel);

            Close();
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void _TagLabel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_TagLabel.SelectedItem != null)
            {
                var tagSearch = GlobalDatabase.TryGetTag((_TagLabel.SelectedItem as Tag).Id);
                if (tagSearch != null)
                    _ColorPicker.SelectedColor = (Color)ColorConverter.ConvertFromString(tagSearch.Color);
            }
        }
    }
}
