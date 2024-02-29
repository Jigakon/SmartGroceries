using SmartGroceries.Models;
using SmartGroceries.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace SmartGroceries.ViewModels
{
    public class ShopArticlesManageViewModel : ViewModelBase, ViewModelManage
    {
        // Navigation
        private readonly Stores.NavigationStore _navigationStore;

        #region Used Data
        public string Name { get; private set; }
        public Guid Id { get; private set; }
        public string Group { get; private set; }
        public string Location { get; private set; }
        private ObservableCollection<ShopArticleViewModel> _shopArticleViewModels;
        // used to Display researches
        private List<Guid> RemovedShopArticles = new List<Guid>();
        #endregion

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

        private ObservableCollection<ShopArticleViewModel> _searchedViewModels;
        public ObservableCollection<ShopArticleViewModel> SearchedViewModels
        {
            get => _searchedViewModels;
            set { _searchedViewModels = value; OnPropertyChanged(nameof(SearchedViewModels)); }
        }

        public ObservableCollection<ShopArticleViewModel> Search(string name, string brand, bool CaseSensitive, bool FullWord)
        {
            ObservableCollection<ShopArticleViewModel> searchList = new ObservableCollection<ShopArticleViewModel>();

            bool nameIsEmpty = string.IsNullOrEmpty(name);
            bool brandIsEmpty = string.IsNullOrEmpty(brand);

            if (nameIsEmpty && brandIsEmpty)
            {
                foreach (var article in _shopArticleViewModels)
                    searchList.Add(article);
            }
            else
            {
                string nameProcessed = CaseSensitive ? name ?? "" : name?.ToLower() ?? "";
                string brandProcessed = CaseSensitive ? brand ?? "" : brand?.ToLower() ?? "";
                foreach (var shopArticle in _shopArticleViewModels)
                {
                    string searchName = CaseSensitive ? shopArticle.Article.Name : shopArticle.Article.Name.ToLower();
                    string searchBrand = CaseSensitive ? shopArticle.Article.Brand : shopArticle.Article.Brand.ToLower();
                    if (FullWord)
                    {
                        var nameTexts = searchName.Split(' ');
                        var brandTexts = searchBrand.Split(' ');
                        foreach (var nameText in nameTexts)
                            if (nameText == nameProcessed || nameIsEmpty)
                                foreach (var brandText in brandTexts)
                                    if (brandText == brandProcessed || brandIsEmpty)
                                        searchList.Add(shopArticle);
                    }
                    else
                    {
                        bool containsName = nameIsEmpty || searchName.Contains(nameProcessed);
                        bool containsBrand = brandIsEmpty || searchBrand.Contains(brandProcessed);
                        if (containsName && containsBrand)
                            searchList.Add(shopArticle);
                    }
                }
            }
            return searchList;
        }
        #endregion

         #region Commands
        public ICommand GoToManageShopsCommand { get; }
        public ICommand AddShopArticleCommand { get; }
        public ICommand SaveArticlesCommand { get; }
        #endregion

        public ShopArticlesManageViewModel(ShopViewModel shopViewModel, Services.NavigationService ShopsManageViewService)
        {
            // Navigation Data
            _navigationStore = ShopsManageViewService.GetNavigationStore();

            // Used Data
            Name = shopViewModel.Name;
            Id = shopViewModel.Id;
            Group = shopViewModel.Group;
            Location = shopViewModel.Location;
            _shopArticleViewModels = new ObservableCollection<ShopArticleViewModel>();
            foreach (var shopArticle in shopViewModel.ShopArticles)
                _shopArticleViewModels.Add(new ShopArticleViewModel(shopArticle.MakeShopArticle(), this, _navigationStore));

            // Commands
            GoToManageShopsCommand = new Commands.NavigateCommand(ShopsManageViewService);
            AddShopArticleCommand = new Commands.MakeShopArticleCommand(this);
            SaveArticlesCommand = new Commands.SaveManageCommand(this);

            SearchedViewModels = _shopArticleViewModels;
        }

        public void AddShopArticle(ShopArticle shopArticle)
        {
            if (_shopArticleViewModels.FirstOrDefault(x => x.Article.Id == shopArticle.Article.Id) == null)
                _shopArticleViewModels.Add(new ShopArticleViewModel(shopArticle, this, _navigationStore));
            Save();
        }

        public Shop MakeShop()
        {
            Shop shop = new Shop(Id, Name, Group, Location);
            foreach (var SAVM in _shopArticleViewModels)
            {
                List<ArticleInfo> articleInfos = new List<ArticleInfo>();
                foreach (var articleInfo in SAVM.ArticleInfos)
                    articleInfos.Add(new ArticleInfo(articleInfo.Price, articleInfo.Date, articleInfo.UnitQuantity));
                shop.TryAddArticle(SAVM.Article, SAVM.ArticleUnit, SAVM.UnitQuantity, articleInfos, SAVM.IsUnitFixed);
            }
            return shop;
        }

        public void Save()
        {
            Shop shop = MakeShop();
            foreach (var shopArticle in _shopArticleViewModels)
                GlobalDatabase.SaveArticle(shopArticle.Article);
            GlobalDatabase.SaveShop(shop);
        }

        public void Remove(ViewModelData viewModel)
        {
            var shopArticle = viewModel as ShopArticleViewModel;
            _shopArticleViewModels.Remove(shopArticle);
            OnPropertyChanged(nameof(SearchedViewModels));
            RemovedShopArticles.Add(shopArticle.Article.Id);
        }
    }
}
