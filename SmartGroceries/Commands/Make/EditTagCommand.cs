using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Commands
{
    internal class EditTagCommand : CommandBase
    {
        private readonly ViewModels.ViewModelBase _containerViewModel;

        public EditTagCommand(ViewModels.ViewModelBase container)
        {
            _containerViewModel = container;
        }

        public override void Execute(object parameter)
        {
            new Windows.EditTagWindow(_containerViewModel)?.ShowDialog();
        }
    }
}
