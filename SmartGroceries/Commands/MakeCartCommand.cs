using SmartGroceries.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Commands
{
    public class MakeCartCommand : CommandBase
    {
        private readonly Stores.NavigationStore _navigationStore;
        private readonly ViewModels.CartsManageViewModel _cartsManageViewModel;

        public MakeCartCommand(ViewModels.CartsManageViewModel cartsManageViewModel)
        {
            _cartsManageViewModel = cartsManageViewModel;
        }

        public override void Execute(object parameter)
        {
            var cartViewModel = new CartViewModel(new Models.Cart(), _navigationStore);
            _cartsManageViewModel.AddCart(cartViewModel);
        }
    }
}
