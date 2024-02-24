using SmartGroceries.Stores;
using SmartGroceries.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartGroceries.Commands
{
    internal class MakeCartArticleCommand : CommandBase
    {
        private readonly ViewModels.CartArticlesManageViewModel _cartManageViewModel;

        public MakeCartArticleCommand(ViewModels.CartArticlesManageViewModel cartManageViewModel)
        {
            _cartManageViewModel = cartManageViewModel;
        }

        public override void Execute(object parameter)
        {
            var cart = _cartManageViewModel.MakeCart();
            if (cart.Shop != null)
            {
                _cartManageViewModel.AddCartArticle(new CartArticleViewModel(new Models.CartArticle(cart), _cartManageViewModel));
            }
            else
                MessageBox.Show("Please select a Shop before adding an article !", "No shop Selected !", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
