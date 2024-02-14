using SmartGroceries.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Services
{
    public class NavigationService
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<ViewModels.ViewModelBase> _createViewModel;

        public NavigationStore GetNavigationStore(){ return _navigationStore; }

        public NavigationService(NavigationStore navigationStore, Func<ViewModels.ViewModelBase> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        public void Navigate()
        {
            if (_navigationStore != null)
                _navigationStore.CurrentViewModel = _createViewModel();
        }
    }
}
