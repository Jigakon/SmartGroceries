using SmartGroceries.ViewModels;
using SmartGroceries.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Commands
{
    public class MakeShopCommand : CommandBase
    {
        private readonly ViewModels.ShopsManageViewModel _shopsManageViewModel;
        private readonly Stores.NavigationStore _navigationStore;

        public MakeShopCommand(ViewModels.ShopsManageViewModel shopsManageViewModel, Stores.NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _shopsManageViewModel = shopsManageViewModel;
        }

        public override void Execute(object parameter)
        {
            _shopsManageViewModel.AddShop(new ViewModels.ShopViewModel(new Models.Shop(), _shopsManageViewModel, _navigationStore));
        }
    }
}
