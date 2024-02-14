using SmartGroceries.Models;
using SmartGroceries.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Commands
{
    public class SaveShopsCommand : CommandBase
    {
        private ViewModelBase _viewModel;

        public SaveShopsCommand(ViewModelBase ViewModel)
        {
            _viewModel = ViewModel;
        }

        public override void Execute(object parameter)
        {
            switch (_viewModel)
            {
                case ShopsManageViewModel shopsManageViewModel:
                    shopsManageViewModel.Save();
                    break;
                case ShopManageViewModel shopManageViewModel:
                    shopManageViewModel.Save();
                    break;
            }

            // temp : save in Json the changes
            GlobalDatabase.SaveTagsInJSON();
            GlobalDatabase.SaveArticlesInJSON();
            GlobalDatabase.SaveShopsInJSON();
        }
    }
}
