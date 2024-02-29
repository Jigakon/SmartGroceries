using SmartGroceries.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SmartGroceries.ViewModels
{
    public class ArticleViewModel : ViewModelData
    {
        public Guid Id { get; set; }

        private string _name;
        public string Name { get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); } }
        private string _brand;
        public string Brand { get => _brand; set { _brand = value; OnPropertyChanged(nameof(Brand)); } }

        private TagViewModel _tag;
        public TagViewModel Tag
        {
            get => _tag;
            set
            {
                _tag = value; OnPropertyChanged(nameof(Tag));
                OnPropertyChanged(nameof(TagColor));
                OnPropertyChanged(nameof(TagTextColor));
                OnPropertyChanged(nameof(TagText));
            }
        }

        public string TagColor { get => Tag?.Color ?? "#FFFFFF"; }
        public string TagTextColor { get => Tag == null ? "#F0F0F0" : "#101010"; }
        public string TagText { get => Tag?.Name ?? "No Tag"; }

        #region Commands
        public ICommand DeleteFromArticleManageCommand { get; }
        public ICommand MakeTagCommand { get; }
        #endregion

        public ArticleViewModel(Models.Article article, ViewModels.ViewModelBase viewModelContainer)
        {
            Id = article.Id;
            Name = article.Name;
            Brand = article.Brand;
            Tag = new TagViewModel(article.Tag);
            ViewModelContainer = viewModelContainer;
            DeleteFromArticleManageCommand = new Commands.RemoveArticleCommand(this);
            MakeTagCommand = new Commands.MakeTagCommand(this);
        }

        public ArticleViewModel(Guid id, string name = "New Article", string brand = "New Article", Tag tag = null)
        {
            Article article = GlobalDatabase.TryGetArticle(id);
            if (article == null)
            {
                Id = id;
                Name = name;
                Brand = brand;
                Tag = new TagViewModel(tag);
            }
            else
            {
                Id = article.Id;
                Name = article.Name;
                Brand = article.Brand;
                Tag = new TagViewModel(article.Tag);
            }
            Tag.ViewModelContainer = this;
            DeleteFromArticleManageCommand = new Commands.RemoveArticleCommand(this);
            MakeTagCommand = new Commands.MakeTagCommand(this);
        }

        public ArticleViewModel(string name = "New Article", string brand = "New Brand", Tag tag = null)
        {
            Id = Guid.NewGuid(); Name = name; Brand = brand;
            Tag = new TagViewModel(tag);

            DeleteFromArticleManageCommand = new Commands.RemoveArticleCommand(this);
            MakeTagCommand = new Commands.MakeTagCommand(this);
        }

        public void SetTag(TagViewModel tagViewModel)
        {
            Tag = tagViewModel;
            OnPropertyChanged(nameof(Tag));
        }

        public Article MakeArticle() => new Article(Id, Name, Brand, Tag.MakeTag());
        public Article MakeArticle(Guid newID) => new Article(newID, Name, Brand, Tag.MakeTag());

        public void Save()
        {
            // article Exists by ID, then By Name and Brand, if it is still not registered create a new Article
            Article article = GlobalDatabase.TryGetArticle(Id) ?? GlobalDatabase.TryGetArticle(Name, Brand) ?? MakeArticle(Guid.NewGuid());
            // article Exists by Name and Brand
            if (article.Name != Name || article.Brand != Brand || article.Tag.Id != Tag.Id)
                if (MessageBox.Show("Modify Article ?", "Article Modified", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    article.Name = Name;
                    article.Brand = Brand;
                    if (Tag != null && Tag.Id != (article.Tag?.Id ?? Guid.Empty))
                        article.Tag = Tag.MakeTag();
                }
                else
                {
                    Name = article.Name;
                    Brand = article.Brand;
                    Tag = new TagViewModel(article.Tag);
                }

            Tag.Save();
            GlobalDatabase.ModifyOrAddArticle(article);
        }
    }
}
