using SmartGroceries.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Commands
{
    public class MakeArticleInfoCommand : CommandBase
    {
        private readonly ViewModels.ArticleInfosManageViewModel _articleInfosManageViewModel;

        public MakeArticleInfoCommand(ViewModels.ArticleInfosManageViewModel articleInfosManageViewModel)
        {
            _articleInfosManageViewModel = articleInfosManageViewModel;
        }

        public override void Execute(object parameter)
        {
            _articleInfosManageViewModel.AddArticleInfo(new ViewModels.ArticleInfoViewModel(_articleInfosManageViewModel));
        }
    }
}
