using SmartGroceries.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Commands
{
    internal class RemoveCartCommand : CommandBase
    {
        private CartViewModel viewModel;

        public RemoveCartCommand(CartViewModel shopViewModel)
        {
            viewModel = shopViewModel;
        }

        public override void Execute(object parameter)
        {
            (viewModel?.ViewModelContainer as CartsManageViewModel)?.Remove(viewModel);
        }
    }
}
