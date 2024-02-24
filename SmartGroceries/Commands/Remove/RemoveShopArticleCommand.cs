using SmartGroceries.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Commands
{
    internal class RemoveShopArticleCommand : CommandBase
    {
        private ShopArticleViewModel viewModel;

        public RemoveShopArticleCommand(ShopArticleViewModel shopViewModel)
        {
            viewModel = shopViewModel;
        }

        public override void Execute(object parameter)
        {
            (viewModel?.ViewModelContainer as ShopArticlesManageViewModel)?.Remove(viewModel);
        }
    }
}
