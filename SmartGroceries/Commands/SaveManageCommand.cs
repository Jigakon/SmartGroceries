using SmartGroceries.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Commands
{
    public class SaveManageCommand : CommandBase
    {
        private ViewModelManage _viewModel;

        public SaveManageCommand(ViewModelManage ViewModel)
        {
            _viewModel = ViewModel;
        }

        public override void Execute(object parameter)
        {
            _viewModel.Save();
        }
    }
}
