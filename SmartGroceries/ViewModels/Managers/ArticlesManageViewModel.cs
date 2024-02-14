using SmartGroceries.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartGroceries.ViewModels
{
    public class ArticlesManageViewModel : ViewModelBase
    {
        private ObservableCollection<ArticleViewModel> _articleViewModels { get; set; }
        public ObservableCollection<ArticleViewModel> SearchedViewModels { get => _articleViewModels; }

        public ICommand MakeArticleCommand { get; }
        public ICommand SaveArticlesCommand { get; }
        public List<Article> Articles
        {
            get
            {
                List<Article> articles = new List<Article>();
                foreach (var article in _articleViewModels)
                {
                    List<Tag> tags = new List<Tag>();
                    foreach (var tagViewModel in article.Tags)
                        tags.Add(new Tag(tagViewModel.Id, tagViewModel.Name, tagViewModel.Color));
                    Article myArticle = GlobalDatabase.GetArticle(article.Id);
                    myArticle.Tags = tags;
                    myArticle.Name = article.Name;
                    myArticle.Brand = article.Brand;
                    articles.Add(myArticle);
                }
                return articles;
            }
        }

        public ArticlesManageViewModel()
        {
            _articleViewModels = new ObservableCollection<ArticleViewModel>();

            MakeArticleCommand = new Commands.MakeArticleCommand(this);
            SaveArticlesCommand = new Commands.SaveArticlesCommand(this);

            ResetArticles();
        }

        public void ResetArticles()
        {
            _articleViewModels.Clear();
            foreach (var article in GlobalDatabase.Instance.Articles)
            {
                ArticleViewModel articleViewModel = new ArticleViewModel(article);
                articleViewModel.ViewModelContainer = this;
                _articleViewModels.Add(articleViewModel);
            }
            OnPropertyChanged(nameof(SearchedViewModels));
        }

        public void AddArticle(ArticleViewModel articleViewModel)
        {
            _articleViewModels.Add(articleViewModel);
            OnPropertyChanged(nameof(SearchedViewModels));
        }

        public void RemoveArticle(ArticleViewModel articleViewModel)
        {
            _articleViewModels.Remove(articleViewModel);
            OnPropertyChanged(nameof(SearchedViewModels));
        }

        public void Save()
        {
            foreach(var article in _articleViewModels)
                article.Save();
        }
    }
}
