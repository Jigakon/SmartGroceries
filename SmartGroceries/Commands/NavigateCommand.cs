using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Commands
{
    public class NavigateCommand : CommandBase
    {
        private readonly Services.NavigationService _navigationService;

        public NavigateCommand(Services.NavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {
            if (_navigationService == null)
                return;

            _navigationService.Navigate();
        }
    }
}
