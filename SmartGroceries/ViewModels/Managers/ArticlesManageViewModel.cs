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
    public class ArticlesManageViewModel : ViewModelBase, ViewModelManage
    {
        private ObservableCollection<ArticleViewModel> _articleViewModels { get; set; }
        public Dictionary<Guid, Tag> _usableTags { get; private set; }
        public List<Tag> UsableTags { get => _usableTags.Values.ToList(); }
        private List<Guid> RemovedArticles = new List<Guid>();

        #region Search
        private string _searchingName;
        public string SearchingName
        {
            get => _searchingName;
            set
            {
                _searchingName = value;
                OnPropertyChanged(nameof(SearchingName));
                SearchedViewModels = Search(SearchingName, SearchingBrand, SearchCaseSensitive, SearchFullWord);
            }
        }

        private string _searchingBrand;
        public string SearchingBrand
        {
            get => _searchingBrand;
            set
            {
                _searchingBrand = value;
                OnPropertyChanged(nameof(SearchingName));
                SearchedViewModels = Search(SearchingName, SearchingBrand, SearchCaseSensitive, SearchFullWord);
            }
        }

        private bool _searchCaseSensitive = false;
        public bool SearchCaseSensitive
        {
            get => _searchCaseSensitive;
            set
            {
                _searchCaseSensitive = value;
                OnPropertyChanged(nameof(SearchCaseSensitive));
                SearchedViewModels = Search(_searchingName, _searchingBrand, SearchCaseSensitive, SearchFullWord);
            }
        }

        private bool _searchFullWord = false;
        public bool SearchFullWord
        {
            get => _searchFullWord;
            set
            {
                _searchFullWord = value;
                OnPropertyChanged(nameof(SearchFullWord));
                SearchedViewModels = Search(_searchingName, _searchingBrand, SearchCaseSensitive, SearchFullWord);
            }
        }

        private ObservableCollection<ArticleViewModel> _searchedViewModels;
        public ObservableCollection<ArticleViewModel> SearchedViewModels
        {
            get => _searchedViewModels;
            set { _searchedViewModels = value; OnPropertyChanged(nameof(SearchedViewModels)); }
        }

        public ObservableCollection<ArticleViewModel> Search(string name, string brand, bool CaseSensitive, bool FullWord)
        {
            ObservableCollection<ArticleViewModel> searchList = new ObservableCollection<ArticleViewModel>();

            bool nameIsEmpty = string.IsNullOrEmpty(name);
            bool brandIsEmpty = string.IsNullOrEmpty(brand);

            if (nameIsEmpty && brandIsEmpty)
            {
                foreach (var article in _articleViewModels)
                    searchList.Add(article);
            }
            else
            {
                string nameProcessed = CaseSensitive ? name ?? "" : name?.ToLower() ?? "";
                string brandProcessed = CaseSensitive ? brand ?? "" : brand?.ToLower() ?? "";
                foreach (var article in _articleViewModels)
                {
                    string searchName = CaseSensitive ? article.Name : article.Name.ToLower();
                    string searchBrand = CaseSensitive ? article.Brand : article.Brand.ToLower();
                    if (FullWord)
                    {
                        var nameTexts = searchName.Split(' ');
                        var brandTexts = searchBrand.Split(' ');
                        foreach (var nameText in nameTexts)
                            if (nameText == nameProcessed || nameIsEmpty)
                                foreach(var brandText in brandTexts)
                                    if (brandText == brandProcessed || brandIsEmpty)
                                        searchList.Add(article);
                    }
                    else
                    {
                        bool containsName = nameIsEmpty || searchName.Contains(nameProcessed);
                        bool containsBrand = brandIsEmpty || searchBrand.Contains(brandProcessed);
                        if (containsName && containsBrand)
                            searchList.Add(article);
                    }
                }
            }
            return searchList;
        }
        #endregion

        #region Commands
        public ICommand MakeArticleCommand { get; }
        public ICommand SaveArticlesCommand { get; }
        #endregion

        public ArticlesManageViewModel()
        {
            _articleViewModels = new ObservableCollection<ArticleViewModel>();

            MakeArticleCommand = new Commands.MakeArticleCommand(this);
            SaveArticlesCommand = new Commands.SaveManageCommand(this);

            _usableTags = new Dictionary<Guid, Tag>();
            foreach (var tag in GlobalDatabase.Instance.Tags)
                _usableTags.Add(tag.Id, tag);

            ResetArticles();
            SearchedViewModels = _articleViewModels;
        }

        public void ResetArticles()
        {
            _articleViewModels.Clear();
            foreach (var article in GlobalDatabase.Instance.Articles)
            {
                ArticleViewModel articleViewModel = new ArticleViewModel(article, this);
                articleViewModel.ViewModelContainer = this;
                _articleViewModels.Add(articleViewModel);
            }
            OnPropertyChanged(nameof(SearchedViewModels));
        }

        public void TryAddTag(TagViewModel tagViewModel)
        {
            if (!_usableTags.ContainsKey(tagViewModel.Id))
                _usableTags.Add(tagViewModel.Id, tagViewModel.MakeTag());
        }

        public void AddArticle(ArticleViewModel articleViewModel)
        {
            _articleViewModels.Add(articleViewModel);
            SearchingBrand = "";
            SearchingName = "";
            OnPropertyChanged(nameof(SearchedViewModels));
        }

        public void Save()
        {
            foreach (var articleID in RemovedArticles)
                GlobalDatabase.RemoveArticle(articleID);
            foreach (var articleViewModel in _articleViewModels)
                articleViewModel.Save();
        }

        public void Remove(ViewModelData viewModel)
        {
            var articleViewModel = viewModel as ArticleViewModel;
            _articleViewModels.Remove(articleViewModel);
            SearchedViewModels.Remove(articleViewModel);
            OnPropertyChanged(nameof(SearchedViewModels));
            RemovedArticles.Add(articleViewModel.Id);
        }
    }
}
