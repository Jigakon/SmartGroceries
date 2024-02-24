using SmartGroceries.Models;
using SmartGroceries.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Logique d'interaction pour EditTagWindow.xaml
    /// </summary>
    public partial class EditTagWindow : Window
    {
        public EditTagWindow(ViewModelBase tagViewModel)
        {
            InitializeComponent();

            _tagViewModel = (tagViewModel as TagViewModel) ?? new TagViewModel(new Models.Tag("New Tag", "#F1F1F1"));
            _ColorPicker.SelectedColor = (Color)ColorConverter.ConvertFromString(_tagViewModel.Color);
            _TagLabelTextBox.Text = _tagViewModel.Name;
        }

        private TagViewModel _tagViewModel;

        private string _tagName;
        public string TagName
        {
            get => _tagName;
            set => _tagName = value;
        }

        private string _tagColor;
        public string TagColor
        {
            get => _tagColor;
            set => _tagColor = value;
        }

        private void SetTagButton_Click(object sender, RoutedEventArgs e)
        {
            if (TagName != "" && (TagColor != _tagViewModel.Color || _tagViewModel.Name != TagName))
            {
                _tagViewModel.Name = TagName;
                _tagViewModel.Color = TagColor;
            }
            Close();
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void _TagLabelTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TagName = _TagLabelTextBox.Text;
        }
        private void _ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            TagColor = _ColorPicker.SelectedColor.ToString();
        }
    }
}
