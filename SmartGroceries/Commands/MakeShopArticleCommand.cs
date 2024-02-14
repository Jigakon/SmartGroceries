using SmartGroceries.ViewModels;
using SmartGroceries.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Commands
{
    internal class MakeShopArticleCommand : CommandBase
    {
        private readonly ViewModels.ShopManageViewModel _shopManageViewModel;

        public MakeShopArticleCommand(ViewModels.ShopManageViewModel shopManageViewModel)
        {
            _shopManageViewModel = shopManageViewModel;
        }

        public override void Execute(object parameter)
        {
            var window = new MakeShopArticleWindow(_shopManageViewModel);
            window.ShowDialog();
        }
    }
}
