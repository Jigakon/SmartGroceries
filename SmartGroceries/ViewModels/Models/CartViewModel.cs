using SmartGroceries.Commands;
using SmartGroceries.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace SmartGroceries.ViewModels
{
    public class CartViewModel : ViewModelData
    {
        private readonly Stores.NavigationStore _navigationStore;

        public Guid Id { get; set; }
        public string Name { get; set; }
        private DateTime _date;
        public DateTime Date { get => _date; set { _date = value.Date; OnPropertyChanged(nameof(Date)); } }
        private Shop _shop;
        public Models.Shop Shop { get => _shop;
            set
            {
                _shop = value;
                OnPropertyChanged(nameof(Shop));
                if (_shop != null)
                {
                    ShopLocation = _shop.Location;
                    OnPropertyChanged(nameof(ShopLocation));
                    ShopName = _shop.Name;
                    OnPropertyChanged(nameof(ShopName));
                }
            }
        }

        private string _shopName;
        public string ShopName { get => _shopName; set { _shopName = value; OnPropertyChanged(nameof(ShopName)); } }
        private string _shopLocation;
        public string ShopLocation { get => _shopLocation; set { _shopLocation = value; OnPropertyChanged(nameof(ShopLocation)); } }
        public ObservableCollection<ViewModels.CartArticleViewModel> CartArticles { get; set; }

        public double totalPrice = 0.0;
        public string TotalPrice { get => totalPrice.ToString("0.00"); }

        public ICommand GoToCartManageCommand { get; }
        public ICommand DeleteFromManage { get; }

        public CartViewModel(Models.Cart cart, ViewModels.ViewModelBase viewModelContainer, Stores.NavigationStore navigationStore) 
        {
            _navigationStore = navigationStore;

            Id = cart.Id;
            Name = cart.Name;
            Date = cart.Date;
            Shop = cart.Shop;
            CartArticles = new ObservableCollection<CartArticleViewModel>();
            foreach(var item in cart.CartArticles)
            {
                totalPrice += (Shop?.GetShopArticle(item.Article.Id)?.GetClosestArticleInfo(Date)?.Price ?? 0) * item.Quantity;
                CartArticles.Add(new CartArticleViewModel(item, viewModelContainer));
            }
            ViewModelContainer = viewModelContainer;
            GoToCartManageCommand = new Commands.NavigateCommand(new Services.NavigationService(navigationStore, CreateCartManageViewModel));
            DeleteFromManage = new Commands.RemoveCartCommand(this);
        }

        public ViewModels.CartArticlesManageViewModel CreateCartManageViewModel()
        {
            return new ViewModels.CartArticlesManageViewModel(this, new Services.NavigationService(_navigationStore, CreateCartsManageViewModel));
        }

        public ViewModels.CartsManageViewModel CreateCartsManageViewModel()
        {
            return new ViewModels.CartsManageViewModel(_navigationStore);
        }

        public Cart MakeCart()
        {
            Cart cart = new Cart(Id, Name, Date, Shop);
            foreach (var CAVM in CartArticles)
                cart.CartArticles.Add(new CartArticle(cart, CAVM.MakeArticle(), CAVM.Quantity, CAVM.Price));
            return cart;
        }

        internal void Save()
        {
            Cart cart = GlobalDatabase.TryGetCart(Id);
            if (cart == null)
                cart = MakeCart();
            else
            {

            }

            GlobalDatabase.ModifyOrAddCart(cart);
        }
    }
}
