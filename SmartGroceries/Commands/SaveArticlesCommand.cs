using SmartGroceries.Models;
using SmartGroceries.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Commands
{
    public class SaveArticlesCommand : CommandBase
    {
        private ArticlesManageViewModel _articlesManageViewModel;

        public SaveArticlesCommand(ArticlesManageViewModel articlesManageViewModel)
        {
            _articlesManageViewModel = articlesManageViewModel;
        }

        public override void Execute(object parameter)
        {
            _articlesManageViewModel.Save();

            // temp : save in Json the changes
            GlobalDatabase.SaveArticlesInJSON();
        }
    }
}
