using SmartGroceries.Models;
using SmartGroceries.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Commands
{
    public class RemoveArticleCommand : CommandBase
    {
        private ArticleViewModel _articleViewModel;

        public RemoveArticleCommand(ArticleViewModel articleViewModel)
        {
            _articleViewModel = articleViewModel;
        }

        public override void Execute(object parameter)
        {
            var articlesManageViewModel = _articleViewModel.ViewModelContainer as ArticlesManageViewModel;
            if (articlesManageViewModel != null) 
            { 
                articlesManageViewModel.RemoveArticle(_articleViewModel);
            }

            // temp : save in Json the changes
            GlobalDatabase.SaveArticlesInJSON();
        }
    }
}
