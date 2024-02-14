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
    public class CartsManageViewModel : ViewModelBase
    {
        private readonly Stores.NavigationStore _navigationStore;

        private ObservableCollection<CartViewModel> _cartViewModels { get; set; }
        public ObservableCollection<CartViewModel> SearchedViewModels { get => _cartViewModels; }

        public ICommand MakeCartCommand { get; set; }
        public ICommand SaveCartsCommand { get; set; }

        public List<Models.Cart> Carts
        {
            get
            {
                List<Models.Cart> carts = new List<Models.Cart>();
                foreach(var item in _cartViewModels)
                {
                    Cart cart = new Cart(item.Id, item.Name, item.Date, item.Shop);
                    List<CartArticle> articles = new List<CartArticle>();
                    foreach(var CAVM in item.CartArticles)
                        articles.Add(new CartArticle(cart, CAVM.article, CAVM.Quantity));
                    cart.CartArticles = articles;
                    carts.Add(cart);
                }
                return carts;
            }
        }

        public CartsManageViewModel(Stores.NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            _cartViewModels = new ObservableCollection<CartViewModel>();

            MakeCartCommand = new Commands.MakeCartCommand(this);
            SaveCartsCommand = new Commands.SaveCartsCommand(this);

            ResetCarts();
        }

        public void ResetCarts()
        {
            _cartViewModels.Clear();
            foreach (var cart in GlobalDatabase.Instance.Carts)
                _cartViewModels.Add(new CartViewModel(cart, _navigationStore));
            OnPropertyChanged(nameof(SearchedViewModels));
        }

        public void AddCart(CartViewModel cart)
        {
            _cartViewModels.Add(cart);
            OnPropertyChanged(nameof(SearchedViewModels));
        }

        public void Save()
        {
            GlobalDatabase.SaveCarts(Carts);
        }
    }
}
