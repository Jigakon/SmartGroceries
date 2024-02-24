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
    /// Logique d'interaction pour MakeTagWindow.xaml
    /// </summary>

    // Big Mess, try to find a way to abstract or 
    public partial class MakeTagWindow : Window
    {
        private TagViewModel _tagViewModel;
        private ViewModelManage _manageViewModel;
        private ObservableCollection<Tag> ExistingTags;

        public string TagName
        {
            get => _tagViewModel.Name;
            set => _tagViewModel.Name = value;
        }
        public string TagColor
        {
            get => _tagViewModel.Color;
            set => _tagViewModel.Color = value;
        }

        public MakeTagWindow(ViewModelBase ContainerViewModel)
        {
            InitializeComponent();

            _tagViewModel = new TagViewModel(new Tag("New Tag", "#FFF0F0F0"));
            _tagViewModel.ViewModelContainer = ContainerViewModel;
            switch (ContainerViewModel)
            {
                case CartArticleViewModel CAVM:
                    if (CAVM.ViewModelContainer is CartArticlesManageViewModel CAMVM)
                    {
                        _manageViewModel = CAMVM;
                        ExistingTags = new ObservableCollection<Tag>(CAMVM.UsableTags);
                    }
                    _TagLabelComboBox.Text = TagName;
                    _TagLabelComboBox.Visibility = Visibility.Visible;
                    _TagLabelTextBox.Visibility = Visibility.Collapsed;
                    break;

                case ArticleViewModel AVM:
                    if (AVM.ViewModelContainer is ArticlesManageViewModel b)
                    {
                        _manageViewModel = b;
                        ExistingTags = new ObservableCollection<Tag>(b.UsableTags);
                    }
                    _TagLabelComboBox.Text = TagName;
                    _TagLabelComboBox.Visibility = Visibility.Visible;
                    _TagLabelTextBox.Visibility = Visibility.Collapsed;
                    break;

                case TagsManageViewModel TMVM:
                    _TagLabelTextBox.Text = TagName;
                    _TagLabelComboBox.Visibility = Visibility.Collapsed;
                    _TagLabelTextBox.Visibility = Visibility.Visible;
                    _manageViewModel = TMVM;
                    ExistingTags = new ObservableCollection<Tag>();
                    break;
            }
            _TagLabelComboBox.ItemsSource = ExistingTags;
        }

        private void SetTagButton_Click(object sender, RoutedEventArgs e)
        {
            if (TagName == "") Close();

            var tagSearch = GlobalDatabase.TryGetTag(TagName, TagColor);
            if (tagSearch != null)
            {
                _tagViewModel.Id = tagSearch.Id;
                TagName = tagSearch.Name;
                TagColor = tagSearch.Color;
            }

            switch (_manageViewModel)
            {
                case CartArticlesManageViewModel CAMVM:
                    (_tagViewModel.ViewModelContainer as CartArticleViewModel)?.SetTag(_tagViewModel);
                    CAMVM.TryAddTag(_tagViewModel);
                    break;

                case ArticlesManageViewModel AMVM:
                    (_tagViewModel.ViewModelContainer as ArticleViewModel)?.SetTag(_tagViewModel);
                    AMVM.TryAddTag(_tagViewModel);
                    break;

                case TagsManageViewModel TMVM:
                    TMVM.AddTag(_tagViewModel);
                    break;
            }

            Close();
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }



        private void _TagLabel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_TagLabelComboBox.SelectedItem != null)
            {
                TagColor = (_TagLabelComboBox.SelectedItem as Tag).Color;
                _ColorPicker.SelectedColor = (Color)ColorConverter.ConvertFromString(TagColor);
            }
        }


        private void _TagLabelTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            switch (sender)
            {
                case TextBox obj: TagName = obj.Text; break;
                case ComboBox obj: TagName = obj.Text; break;
            }
        }

        private void _ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            TagColor = _ColorPicker.SelectedColor.ToString();
        }
    }
}
