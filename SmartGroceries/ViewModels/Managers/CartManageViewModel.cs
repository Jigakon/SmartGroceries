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
    public class CartManageViewModel : ViewModelBase
    {
        public CartViewModel CartViewModel { get; set; }

        public string Name => CartViewModel.Name;
        public DateTime Date
        {
            get => CartViewModel.Date;
            set { CartViewModel.Date = value.Date; }
        }

        public ObservableCollection<CartArticleViewModel> cartArticles { get; set; }

        public ObservableCollection<CartArticleViewModel> SearchedViewModels { get => cartArticles; }

        public ICommand GoToManageCartsCommand { get; }
        public ICommand AddCartArticleCommand { get; }
        public ICommand SaveCartArticlesCommand { get; }
        public Shop Shop { get; set; }

        public List<ShopArticle> shopArticles => Shop.ShopArticles;

        public CartManageViewModel(CartViewModel cartViewModel, Services.NavigationService CartsManageViewService)
        {
            CartViewModel = cartViewModel;
            Shop = cartViewModel.Shop;
            cartArticles = new ObservableCollection<CartArticleViewModel>();
            foreach (var cartArticle in cartViewModel.CartArticles)
                cartArticles.Add(cartArticle);

            GoToManageCartsCommand = new Commands.NavigateCommand(CartsManageViewService);
            AddCartArticleCommand = new Commands.MakeCartArticleCommand(this);
            SaveCartArticlesCommand = new Commands.SaveCartsCommand(this);
        }

        public void AddCartArticle(CartArticle cartArticle)
        {
            cartArticles.Add(new CartArticleViewModel(cartArticle));
        }

        public Cart MakeCart()
        {
            Cart cart = new Cart(CartViewModel.Id, CartViewModel.Name, CartViewModel.Date, CartViewModel.Shop);
            foreach (var CAVM in cartArticles)
                cart.AddCartArticle(new CartArticle(cart, CAVM.article, CAVM.Quantity, CAVM.Price));
            return cart;
        }

        public void Save()
        {
            Cart cart = MakeCart();

            GlobalDatabase.SaveCart(cart);
        }
    }
}
