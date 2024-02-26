using SmartGroceries.Commands;
using SmartGroceries.Models;
using SmartGroceries.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace SmartGroceries.ViewModels
{
    public class SetUnitCommand : CommandBase
    {
        ShopArticleViewModel _shopArticleViewModel;
        Unit _unitToSet;
        public SetUnitCommand(ShopArticleViewModel shopArticleViewModel, Unit unit)
        {
            _shopArticleViewModel = shopArticleViewModel;
            _unitToSet = unit;
        }

        public override void Execute(object parameter)
        {
            _shopArticleViewModel.ArticleUnit = _unitToSet;
        }
    }

    public class ShopArticleViewModel : ViewModelData
    {
        private readonly Stores.NavigationStore _navigationStore;

        public Article Article { get; set; }
        public string ArticleTagName => Article.Tag?.Name ?? "";
        public string ArticleTagColor => Article.Tag?.Color ?? App.GetColorAsString("AccentColor");
        public Shop Shop { get; set; }
        public List<ArticleInfo> ArticleInfos { get; set; }
        private Unit _articleUnit;
        public Unit ArticleUnit { get => _articleUnit; set { _articleUnit = value; OnPropertyChanged(nameof(ArticleUnit)); OnPropertyChanged(nameof(UnitText)); } }
        private float _unitQuantity;
        public float UnitQuantity { get => IsUnitFixed ? lastShopArticle.UnitQuantity : _unitQuantity; 
            set 
            {
                if (IsUnitFixed) return;

                _unitQuantity = value; 
                OnPropertyChanged(nameof(UnitQuantity)); 
                OnPropertyChanged(nameof(lastArticlePriceByUnitString)); 
            } 
        }

        private bool _isUnitFixed = false;
        public bool IsUnitFixed
        {
            get => _isUnitFixed; 
            set 
            { 
                _isUnitFixed = value; 
                OnPropertyChanged(nameof(IsUnitFixed));
                OnPropertyChanged(nameof(UnitQuantity));
            }
        }

        public string UnitText
        {
            get
            {
                switch(_articleUnit)
                {
                    case Unit.Piece: return "Piece";
                    case Unit.Weight: return "kg";
                    case Unit.Volume: return "l";
                    default: return "";
                }
            }
        }

        public ICommand GoToArticleInfoManageCommand { get; }
        public ICommand SetWeightCommand { get; }
        public ICommand SetVolumeCommand { get; }
        public ICommand SetPieceCommand { get; }
        public ICommand DeleteFromShopManageCommand { get; }

        public ArticleInfo lastShopArticle => (ArticleInfos != null && ArticleInfos.Count != 0) ? ArticleInfos.Last() : new ArticleInfo();
        public string lastArticlePriceString { get => ArticleInfos.Count != 0 ? lastShopArticle.Price.ToString() : "-1"; }
        public string lastArticlePriceByUnitString { get => ArticleInfos.Count != 0 ? (lastShopArticle.Price / (UnitQuantity == 0 ? 1 : UnitQuantity)).ToString("0.000") : "-1"; }

        public ShopArticleViewModel(ShopArticle shopArticle, ViewModelBase viewModelContainer, Stores.NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            Article = shopArticle.Article;
            Shop = shopArticle.Shop;

            ArticleUnit = shopArticle.ArticleUnit;

            UnitQuantity = shopArticle.UnitQuantity;
            IsUnitFixed = shopArticle.IsUnitFixed;

            ArticleInfos = new List<ArticleInfo>();
            foreach(var articleInfo in shopArticle.ArticleInfos) 
                ArticleInfos.Add(articleInfo);

            ViewModelContainer = viewModelContainer;

            GoToArticleInfoManageCommand = new Commands.NavigateCommand(new Services.NavigationService(_navigationStore, CreateArticleInfosManageViewModel));
            DeleteFromShopManageCommand = new Commands.RemoveShopArticleCommand(this);
            SetWeightCommand = new SetUnitCommand(this, Unit.Weight);
            SetVolumeCommand = new SetUnitCommand(this, Unit.Volume);
            SetPieceCommand = new SetUnitCommand(this, Unit.Piece);
        }

        public ShopArticle MakeShopArticle()
        {
            return new ShopArticle(Shop, Article, ArticleInfos, ArticleUnit, UnitQuantity, IsUnitFixed);
        }

        public void Save()
        {
            ShopArticle shopArticle = Shop.GetShopArticle(Article.Id);
            if (shopArticle == null)
            {
                shopArticle = MakeShopArticle();
                Shop.TryAddArticle(shopArticle);
            }
            else
            {
                shopArticle.Article.Name = Article.Name;
                shopArticle.ArticleUnit = ArticleUnit;
                shopArticle.UnitQuantity = UnitQuantity;
                shopArticle.IsUnitFixed = IsUnitFixed;
            }
            
            Article article = GlobalDatabase.TryGetArticle(shopArticle.Article.Id);
            if (article == null)
                article = shopArticle.Article;

            GlobalDatabase.TryAddTag(article.Tag);
            // save Article
            GlobalDatabase.ModifyOrAddArticle(article);
            
        }

        public ArticleInfosManageViewModel CreateArticleInfosManageViewModel()
        {
            return new ArticleInfosManageViewModel(this, new Services.NavigationService(_navigationStore, CreateShopManageViewModel));
        }

        public ShopArticlesManageViewModel CreateShopManageViewModel() 
        {
            var shopViewModel = new ShopViewModel(Shop, ViewModelContainer, _navigationStore);
            return new ShopArticlesManageViewModel(shopViewModel, new Services.NavigationService(_navigationStore, shopViewModel.CreateShopsManageViewModel));
        }
    }
}
