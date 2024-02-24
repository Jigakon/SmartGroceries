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
            (_articleViewModel?.ViewModelContainer as ArticlesManageViewModel)?.Remove(_articleViewModel);
        }
    }
}
