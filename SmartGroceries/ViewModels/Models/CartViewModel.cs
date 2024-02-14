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
    public class CartViewModel : ViewModelBase
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
        public string ShopName { get; set; }
        public string ShopLocation { get; private set; }
        public ObservableCollection<ViewModels.CartArticleViewModel> CartArticles { get; set; }

        public ICommand GoToCartManageCommand { get; }

        public CartViewModel(Models.Cart cart, Stores.NavigationStore navigationStore) 
        {
            _navigationStore = navigationStore;

            Id = cart.Id;
            Name = cart.Name;
            Date = cart.Date;
            Shop = cart.Shop;
            CartArticles = new ObservableCollection<CartArticleViewModel>();
            foreach(var item in cart.CartArticles)
            {
                CartArticles.Add(new CartArticleViewModel(item));
            }
            GoToCartManageCommand = new Commands.NavigateCommand(new Services.NavigationService(navigationStore, CreateCartManageViewModel));
        }

        public ViewModels.CartManageViewModel CreateCartManageViewModel()
        {
            return new ViewModels.CartManageViewModel(this, new Services.NavigationService(_navigationStore, CreateCartsManageViewModel));
        }

        public ViewModels.CartsManageViewModel CreateCartsManageViewModel()
        {
            return new ViewModels.CartsManageViewModel(_navigationStore);
        }
    }
}
