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

        public MakeCartCommand(ViewModels.CartsManageViewModel cartsManageViewModel, Stores.NavigationStore navigationStore)
        {
            _cartsManageViewModel = cartsManageViewModel;
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            _cartsManageViewModel.AddCart(new ViewModels.CartViewModel(new Models.Cart(), _cartsManageViewModel, _navigationStore));
        }
    }
}
