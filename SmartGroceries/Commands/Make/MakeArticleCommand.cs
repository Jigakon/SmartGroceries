using SmartGroceries.Models;
using SmartGroceries.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Commands
{
    public class MakeArticleCommand : CommandBase
    {
        private readonly ViewModels.ArticlesManageViewModel _articlesManageViewModel;

        public MakeArticleCommand(ViewModels.ArticlesManageViewModel articlesManageViewModel)
        {
            _articlesManageViewModel = articlesManageViewModel;
        }

        public override void Execute(object parameter)
        {
            _articlesManageViewModel.AddArticle(new ViewModels.ArticleViewModel(new Article(), _articlesManageViewModel));
        }
    }
}
