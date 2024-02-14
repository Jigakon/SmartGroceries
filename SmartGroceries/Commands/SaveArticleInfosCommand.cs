using SmartGroceries.Models;
using SmartGroceries.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Commands
{
    public class SaveArticleInfosCommand : CommandBase
    {
        private ArticleInfosManageViewModel _articleInfosManageViewModel;

        public SaveArticleInfosCommand(ArticleInfosManageViewModel articleInfosManageViewModel)
        {
            _articleInfosManageViewModel = articleInfosManageViewModel;
        }

        public override void Execute(object parameter)
        {
            _articleInfosManageViewModel.Save();

            // temp : save in Json the changes
            GlobalDatabase.SaveArticlesInJSON();
            GlobalDatabase.SaveShopsInJSON();
        }
    }
}
