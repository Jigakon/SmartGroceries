using SmartGroceries.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SmartGroceries.ViewModels
{
    public class CartsManageViewModel : ViewModelBase, ViewModelManage
    {


        private readonly Stores.NavigationStore _navigationStore;

        private ObservableCollection<CartViewModel> _cartViewModels { get; set; }
        public ObservableCollection<CartViewModel> SearchedViewModels { get => _cartViewModels; }
        private List<Guid> RemovedCarts = new List<Guid>();

        #region Commands
        public ICommand MakeCartCommand { get; set; }
        public ICommand SaveCartsCommand { get; set; }
        #endregion

        public CartsManageViewModel(Stores.NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            _cartViewModels = new ObservableCollection<CartViewModel>();

            MakeCartCommand = new Commands.MakeCartCommand(this, _navigationStore);
            SaveCartsCommand = new Commands.SaveManageCommand(this);


            ResetCarts();
        }

        private DateTime SortByDate(CartViewModel elem) => elem.Date;

        public void ResetCarts()
        {
            _cartViewModels.Clear();
            foreach (var cart in GlobalDatabase.Instance.Carts)
            {
                _cartViewModels.Add(new CartViewModel(cart, this, _navigationStore));
            }
            _cartViewModels = new ObservableCollection<CartViewModel>(_cartViewModels.OrderByDescending(SortByDate));
            OnPropertyChanged(nameof(SearchedViewModels));
        }

        public void AddCart(CartViewModel cart)
        {
            _cartViewModels.Add(cart);
            OnPropertyChanged(nameof(SearchedViewModels));
            Save();
        }

        public void Save()
        {
            foreach (var cartID in RemovedCarts)
                GlobalDatabase.RemoveCart(cartID);

            foreach (var cartViewModel in _cartViewModels)
            {
                Cart cart = GlobalDatabase.TryGetCart(cartViewModel.Id);
                if (cart != null)
                {
                    if (cart.Name != cartViewModel.Name)
                    {
                        if (MessageBoxResult.OK == MessageBox.Show($"Change name {cart.Name} by {cartViewModel.Name}", "Cart exists !", MessageBoxButton.OKCancel))
                            cart.Name = cartViewModel.Name;
                        else
                            cartViewModel.Name = cart.Name;
                    }

                    if (cart.Date != cartViewModel.Date)
                    {
                        if (MessageBoxResult.OK == MessageBox.Show($"Change Date {cart.Date} by {cartViewModel.Date}"))
                            cart.Date = cartViewModel.Date;
                        else
                            cartViewModel.Date = cart.Date;
                    }

                    if (cartViewModel.Shop == null || GlobalDatabase.TryGetShop(cartViewModel.Shop.Id) == null)
                    {
                        if (MessageBoxResult.OK == MessageBox.Show("Shop doesn't Exists !", "", MessageBoxButton.OKCancel))
                        {
                            cart.Shop = new Shop(cartViewModel.ShopName);
                            new Windows.MakeShopWindow(cartViewModel).ShowDialog();
                            cartViewModel.Save();
                        }
                    }
                    else
                        cart.Shop = cartViewModel.Shop;
                }
                else
                    GlobalDatabase.AddCart(cartViewModel.MakeCart());
            }
        }

        public void Remove(ViewModelData viewModel)
        {
            var cartViewModel = viewModel as CartViewModel;
            _cartViewModels.Remove(cartViewModel);
            OnPropertyChanged(nameof(SearchedViewModels));
            RemovedCarts.Add(cartViewModel.Id);
        }
    }
}
