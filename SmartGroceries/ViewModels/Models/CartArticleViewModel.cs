using SmartGroceries.Commands;
using SmartGroceries.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace SmartGroceries.ViewModels
{
    public class CartArticleViewModel : ViewModelData
    {
        private List<ShopArticle> _allArticles;
        private List<ShopArticle> _selectedSource;

        private IEnumerable<ShopArticle> _searchedSource;
        public IEnumerable<ShopArticle> SearchedSource
        {
            get => _searchedSource;
            set
            {
                _searchedSource = value;
                OnPropertyChanged(nameof(SearchedSource));
            }
        }

        private Models.ShopArticle _selectedShopArticle;
        public Models.ShopArticle SelectedShopArticle
        {
            get => _selectedShopArticle;
            set
            {
                _selectedShopArticle = value;
                OnPropertyChanged(nameof(SelectedShopArticle));

                if (SelectedShopArticle != null)
                {
                    Price = _selectedShopArticle?.GetClosestArticleInfo(Date)?.Price ?? 0;
                    Tag = _selectedShopArticle.Article.Tag;
                    Brand = _selectedShopArticle.Article.Brand;

                    _articleName = _selectedShopArticle.Article.Name;
                    OnPropertyChanged(nameof(ArticleName));
                    OnPropertyChanged(nameof(SearchedSource));
                }
            }
        }

        #region Article
        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value; 
                OnPropertyChanged(nameof(Date));
            }
        }

        private Shop _shop;
        public Shop Shop
        {
            get => _shop;
            set
            {
                _shop = value;
                OnPropertyChanged(nameof(Shop));
            }
        }

        private string _brand;
        public string Brand
        {
            get => _brand;
            set
            {
                _brand = value;
                OnPropertyChanged(nameof(Brand));
            }
        }

        private IEnumerable<ShopArticle> Search(bool caseSensitive = false)
        {
            if (caseSensitive)
                return _selectedSource.Where(x => x.Article.Name.Contains(_articleName)).OrderBy(x => x.Article.Name);
            string nameLower = _articleName.ToLower();
            return _selectedSource.Where(x => x.Article.Name.Contains(nameLower)).OrderBy(x => x.Article.Name);
        }

        private string _articleName;
        public string ArticleName
        {
            get => _articleName;
            set
            {
                _articleName = value;
                OnPropertyChanged(nameof(ArticleName));
                SearchedSource = Search();
            }
        }

        private Tag _tag;
        public Tag Tag
        {
            get => _tag;
            set
            {
                _tag = value;
                OnPropertyChanged(nameof(Tag));
                OnPropertyChanged(nameof(TagName));
                OnPropertyChanged(nameof(TagColor));
            }
        }

        public void SetTag(TagViewModel tagViewModel)
        {
            Tag = tagViewModel.MakeTag();
        }

        public string TagName => Tag?.Name ?? "";
        public string TagColor => Tag?.Color ?? App.GetColorAsString("AccentColor");

        public Article MakeArticle()
        {
            Article search = GlobalDatabase.TryGetArticle(ArticleName, Brand);
            if (search != null)
                return new Article(search);
            return new Article(ArticleName, Brand, Tag);
        }
        #endregion

        private uint _quantity;
        public uint Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(TotalPrice));
                (ViewModelContainer as CartArticlesManageViewModel)?.UpdateTotalPrice();
            }
        }

        private float _price;
        public float Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(TotalPrice));
                (ViewModelContainer as CartArticlesManageViewModel)?.UpdateTotalPrice();
            }
        }

        public string TotalPrice => (Price * Quantity).ToString();

        private bool _changeSelection;
        public bool ChangeSelection
        {
            get => _changeSelection;
            set
            {
                _changeSelection = value;
                OnPropertyChanged(nameof(ChangeSelection));
                _selectedSource = value ? _allArticles : Shop.ShopArticles.Values.ToList();
                SearchedSource = Search();
            }
        }

        private Unit _articleUnit;
        public Unit ArticleUnit { get => _articleUnit; set { _articleUnit = value; OnPropertyChanged(nameof(ArticleUnit)); OnPropertyChanged(nameof(UnitText)); } }
        private float _unitQuantity;
        public float UnitQuantity { get => _unitQuantity; set { _unitQuantity = value; OnPropertyChanged(nameof(UnitQuantity)); } }

        private bool _isUnitFixed = false;
        public bool IsUnitFixed
        {
            get => _isUnitFixed;
            set
            {
                _isUnitFixed = value;
                OnPropertyChanged(nameof(IsUnitFixed));
                OnPropertyChanged(nameof(IsNotUnitFixed));
            }
        }
        public bool IsNotUnitFixed => !_isUnitFixed;

        public string UnitText
        {
            get
            {
                switch (_articleUnit)
                {
                    case Unit.Piece: return "Piece";
                    case Unit.Weight: return "kg";
                    case Unit.Volume: return "l";
                    default: return "";
                }
            }
        }

        public ICommand DeleteFromManage { get; }
        public ICommand MakeTagCommand { get; }

        public CartArticleViewModel(Models.CartArticle cartArticle, ViewModels.ViewModelBase viewModelContainer)
        {
            Shop = cartArticle.Cart.Shop;
            IsNew = false;
            if (Shop.ShopArticles.TryGetValue(cartArticle.Article.Id, out var shopArticle))
            {
                IsUnitFixed = shopArticle.IsUnitFixed;
                if (IsUnitFixed)
                    UnitQuantity = shopArticle.UnitQuantity;
                ArticleUnit = shopArticle.ArticleUnit;
            }
            else
            {
                IsUnitFixed = false;
                ArticleUnit = Unit.Weight;
            }

            _allArticles = new List<ShopArticle>();
            if (Shop != null)
                foreach (var article in GlobalDatabase.Instance.Articles)
                {
                    Shop.ShopArticles.TryGetValue(article.Id, out ShopArticle search);
                    if (search == null)
                        search = new ShopArticle(Shop, article);
                    _allArticles.Add(search);
                }
            _selectedSource = Shop?.ShopArticles.Values.ToList();

            Brand = cartArticle.Article.Brand;
            ArticleName = cartArticle.Article.Name;
            Quantity = cartArticle.Quantity;
            Tag = cartArticle.Article.Tag;

            SelectedShopArticle = cartArticle.Cart.Shop?.GetShopArticle(cartArticle.Article.Id);
            Price = SelectedShopArticle?.GetArticleInfo(cartArticle.Cart.Date)?.Price ?? 0f;
            ViewModelContainer = viewModelContainer;

            DeleteFromManage = new RemoveCartArticleCommand(this);
            MakeTagCommand = new MakeTagCommand(this);
        }
    }
}
