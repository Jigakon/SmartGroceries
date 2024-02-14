using SmartGroceries.Stores;
using SmartGroceries.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Commands
{
    internal class MakeCartArticleCommand : CommandBase
    {
        private readonly ViewModels.CartManageViewModel _cartManageViewModel;

        public MakeCartArticleCommand(ViewModels.CartManageViewModel cartManageViewModel)
        {
            _cartManageViewModel = cartManageViewModel;
        }

        public override void Execute(object parameter)
        {
            _cartManageViewModel.AddCartArticle(new Models.CartArticle(_cartManageViewModel.MakeCart()));
        }
    }
}
