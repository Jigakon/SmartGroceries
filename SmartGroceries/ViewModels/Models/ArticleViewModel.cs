using SmartGroceries.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartGroceries.ViewModels
{
    public class ArticleViewModel : ViewModelBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public ObservableCollection<TagViewModel> Tags { get; set; }
        public ViewModels.ViewModelBase ViewModelContainer { get; set; }
        public ICommand DeleteFromArticleManageCommand { get; }
        public ICommand MakeTagCommand { get; }
        public ArticleViewModel(Models.Article article)
        {
            Id = article.Id; Name = article.Name; Brand = article.Brand;
            Tags = new ObservableCollection<TagViewModel>();
            foreach (var item in article.Tags)
            {
                var tagViewModel = new TagViewModel(item);
                tagViewModel.ViewModelContainer = this;
                Tags.Add(tagViewModel);
            }

            DeleteFromArticleManageCommand = new Commands.RemoveArticleCommand(this);
            MakeTagCommand = new Commands.MakeTagCommand(this);
        }

        public ArticleViewModel(Guid id, string name = "New Article", string brand = "New Article", List<Tag> tags = null)
        {
            Tags = new ObservableCollection<TagViewModel>();
            Article article = GlobalDatabase.GetArticle(id);
            if (article != null)
            {
                Id = id; Name = name; Brand = brand;
                if (tags != null)
                    foreach (var item in tags)
                    {
                        var tagViewModel = new TagViewModel(item);
                        tagViewModel.ViewModelContainer = this;
                        Tags.Add(tagViewModel);
                    }
            }
            else
            {
                Id = article.Id; Name = article.Name; Brand = article.Brand;
                if (article.Tags != null)
                    foreach (var item in article.Tags)
                    { 
                        var tagViewModel = new TagViewModel(item);
                        tagViewModel.ViewModelContainer = this;
                        Tags.Add(tagViewModel);
                    }
            }
            DeleteFromArticleManageCommand = new Commands.RemoveArticleCommand(this);
            MakeTagCommand = new Commands.MakeTagCommand(this);
        }

        public ArticleViewModel(string name = "New Article", string brand = "New Brand", List<Tag> tags = null)
        {
            Id = Guid.NewGuid(); Name = name; Brand = brand;
            Tags = new ObservableCollection<TagViewModel>();
            if (tags != null)
                foreach (var item in tags)
                {
                    var tagViewModel = new TagViewModel(item);
                    tagViewModel.ViewModelContainer = this;
                    Tags.Add(tagViewModel);
                }

            DeleteFromArticleManageCommand = new Commands.RemoveArticleCommand(this);
            MakeTagCommand = new Commands.MakeTagCommand(this);
        }

        public void AddTag(TagViewModel tagViewModel)
        {
            if (Tags.FirstOrDefault(x => x.Id == tagViewModel.Id) != null)
                return;
            Tags.Add(tagViewModel);
            OnPropertyChanged(nameof(Tags));
        }

        public void RemoveTag(TagViewModel tagViewModel)
        {
            Tags.Remove(tagViewModel);
            OnPropertyChanged(nameof(Tags));
        }

        public void Save(bool Override = true)
        {
            Article article = GlobalDatabase.GetArticle(Id);
            if (article == null)
            {
                List<Tag> tags = new List<Tag>();
                foreach (var item in Tags)
                {
                    item.Save(Override);
                    tags.Add(item.MakeTag());
                }
                GlobalDatabase.TryAddArticle(new Article(Id, Name, Brand, tags));
            }
            else if (Override)
            {
                article.Id = Id;
                article.Name = Name;
                article.Brand = Brand;
                List<Tag> tags = new List<Tag>();
                foreach (var item in Tags)
                {
                    item.Save(Override);
                    tags.Add(item.MakeTag());
                }
                article.Tags = tags;
            }
        }
    }
}
