using SmartGroceries.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartGroceries.ViewModels
{
    public class CartArticlesManageViewModel : ViewModelBase, ViewModelManage
    {
        public Guid CartId { get; private set; }
        private string _cartName;
        public string CartName { get => _cartName; set { _cartName = value; OnPropertyChanged(nameof(CartName)); } }
        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set { _date = value.Date; }
        }
        public Dictionary<Guid, Tag> _usableTags { get; private set; }
        public List<Tag> UsableTags { get => _usableTags.Values.ToList(); }
        public ObservableCollection<CartArticleViewModel> cartArticleViewModels { get; set; }

        public ObservableCollection<CartArticleViewModel> SearchedViewModels { get => cartArticleViewModels; }
        private List<Guid> RemovedCartArticles = new List<Guid>();
        
        private Shop _shop;
        public Shop Shop 
        { 
            get => _shop; 
            set
            {
                _shop = value;
                OnPropertyChanged(nameof(Shop));
                ChangeCartArticleShop(Shop);
            }
        }

        public bool ContainsArticle(string ArticleName, string ArticleBrand, bool caseSensitive = false)
        {
            if (caseSensitive)
                return cartArticleViewModels.FirstOrDefault(x => x.ArticleName == ArticleName && x.Brand == ArticleBrand) != null;
            string ArticleNameLower = ArticleName.ToLower(), ArticleBrandLower = ArticleBrand.ToLower();
            return cartArticleViewModels.FirstOrDefault(x => x.ArticleName.ToLower() == ArticleNameLower && x.Brand.ToLower() == ArticleBrandLower) != null;
        }

        public string TotalPrice
        {
            get
            {
                float price = 0;
                foreach (var cartArticle in cartArticleViewModels)
                    price += cartArticle.Price * cartArticle.Quantity;
                return price.ToString("0.00");
            }
        }
        public void UpdateTotalPrice() => OnPropertyChanged(nameof(TotalPrice));
 
        #region Commands
        public ICommand GoToManageCartsCommand { get; }
        public ICommand AddCartArticleCommand { get; }
        public ICommand SaveCartArticlesCommand { get; }
        #endregion

        public CartArticlesManageViewModel(CartViewModel cartViewModel, Services.NavigationService CartsManageViewService)
        {
            CartId = cartViewModel.Id;
            cartArticleViewModels = new ObservableCollection<CartArticleViewModel>();
            Shop = cartViewModel.Shop;
            CartName = cartViewModel.Name;
            Date = cartViewModel.Date;
            foreach (var cartArticle in cartViewModel.CartArticles)
                cartArticleViewModels.Add(new CartArticleViewModel(new CartArticle(cartViewModel.MakeCart(), cartArticle.MakeArticle(), cartArticle.Quantity, cartArticle.Price), this));

            _usableTags = new Dictionary<Guid, Tag>();
            foreach (var tag in GlobalDatabase.Instance.Tags)
                _usableTags.Add(tag.Id, tag);

            GoToManageCartsCommand = new Commands.NavigateCommand(CartsManageViewService);
            AddCartArticleCommand = new Commands.MakeCartArticleCommand(this);
            SaveCartArticlesCommand = new Commands.SaveManageCommand(this);
        }

        private void ChangeCartArticleShop(Shop shop)
        {
            foreach (var cartArticleViewModel in cartArticleViewModels)
                cartArticleViewModel.Shop = shop;
        }

        public void AddCartArticle(CartArticleViewModel cartArticle)
        {
            if (!ContainsArticle(cartArticle.ArticleName, cartArticle.Brand))
                cartArticleViewModels.Add(cartArticle);
        }

        public void TryAddTag(TagViewModel tagViewModel)
        {
            if (!_usableTags.ContainsKey(tagViewModel.Id))
                _usableTags.Add(tagViewModel.Id, tagViewModel.MakeTag());
        }

        public Cart MakeCart()
        {
            Cart cart = new Cart(CartId, CartName, Date, Shop);
            foreach (var CAVM in cartArticleViewModels)
                cart.AddCartArticle(new CartArticle(cart, CAVM.MakeArticle(), CAVM.Quantity, CAVM.Price));
            return cart;
        }

        public void Save()
        {
            

            Cart cart = MakeCart();

            GlobalDatabase.SaveCart(cart);
        }

        public void Remove(ViewModelData viewModel)
        {
            var cartArticle = viewModel as CartArticleViewModel;
            cartArticleViewModels.Remove(cartArticle);
            OnPropertyChanged(nameof(SearchedViewModels));
            RemovedCartArticles.Add(cartArticle.MakeArticle().Id);
        }
    }
}
