using SmartGroceries.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Commands
{
    internal class RemoveCartArticleCommand : CommandBase
    {
        private CartArticleViewModel viewModel;

        public RemoveCartArticleCommand(CartArticleViewModel ViewModel)
        {
            viewModel = ViewModel;
        }

        public override void Execute(object parameter)
        {
            (viewModel?.ViewModelContainer as CartArticlesManageViewModel)?.Remove(viewModel);
        }
    }
}
