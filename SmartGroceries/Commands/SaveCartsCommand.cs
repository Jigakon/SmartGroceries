using SmartGroceries.Models;
using SmartGroceries.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Commands
{
    public class SaveCartsCommand : CommandBase
    {
        private ViewModelBase _viewModel;
        

        public SaveCartsCommand(ViewModelBase viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            switch(_viewModel)
            {
                case CartsManageViewModel cartsManageViewModel:
                    cartsManageViewModel.Save();
                    break;
                case CartManageViewModel cartManageViewModel:
                    cartManageViewModel.Save();
                    break;
            }

            // temp : save in Json the changes
            GlobalDatabase.SaveTagsInJSON();
            GlobalDatabase.SaveArticlesInJSON();
            GlobalDatabase.SaveShopsInJSON();
            GlobalDatabase.SaveCartsInJSON();
        }
    }
}
