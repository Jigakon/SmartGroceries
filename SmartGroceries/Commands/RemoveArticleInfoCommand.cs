using SmartGroceries.Models;
using SmartGroceries.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Commands
{
    internal class RemoveArticleInfoCommand : CommandBase
    {
        private ArticleInfoViewModel _viewModel;

        public RemoveArticleInfoCommand(ArticleInfoViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            if (_viewModel != null)
            {
                (_viewModel.ViewModelContainer as ArticleInfosManageViewModel).RemoveArticleInfo(_viewModel);
            }

            // temp : save in Json the changes
            GlobalDatabase.SaveShopsInJSON();
        }
    }
}
