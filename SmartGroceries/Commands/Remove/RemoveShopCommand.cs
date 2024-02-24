using SmartGroceries.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Commands
{
    internal class RemoveShopCommand : CommandBase
    {
        private ShopViewModel _shopViewModel;

        public RemoveShopCommand(ShopViewModel shopViewModel)
        {
            _shopViewModel = shopViewModel;
        }

        public override void Execute(object parameter)
        {
            (_shopViewModel?.ViewModelContainer as ShopsManageViewModel)?.Remove(_shopViewModel);
        }
    }
}
