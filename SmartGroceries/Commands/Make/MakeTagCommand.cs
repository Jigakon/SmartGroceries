using SmartGroceries.ViewModels;
using SmartGroceries.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Commands
{
    public class MakeTagCommand : CommandBase
    {
        private readonly ViewModels.ViewModelBase _containerViewModel;

        public MakeTagCommand(ViewModels.ViewModelBase container)
        {
            _containerViewModel = container;
        }

        public override void Execute(object parameter)
        {
            new Windows.MakeTagWindow(_containerViewModel)?.ShowDialog();
        }
    }
}
