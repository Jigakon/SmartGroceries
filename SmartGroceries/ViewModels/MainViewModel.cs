using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartGroceries.ViewModels
{   public class MainViewModel : ViewModelBase
    {
        private readonly Stores.NavigationStore _navigationStore;
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public ICommand GoToManageTagsCommand { get; }
        public ICommand GoToManageArticlesCommand { get; }
        public ICommand GoToManageShopsCommand { get; }
        public ICommand GoToManageCartsCommand { get; }
        public ICommand GoToCompareCommand { get; }
        public ICommand GoToCartsInformationCommand { get; }

        public MainViewModel(Stores.NavigationStore navigationStore,
            Services.NavigationService TagsManageViewService,
            Services.NavigationService ArticlesManageViewService,
            Services.NavigationService ShopsManageViewService,
            Services.NavigationService CartsManageViewService,
            Services.NavigationService CompareViewService,
            Services.NavigationService CartsInformationService)
        {
            _navigationStore = navigationStore;

            GoToManageTagsCommand = new Commands.NavigateCommand(TagsManageViewService);
            GoToManageArticlesCommand = new Commands.NavigateCommand(ArticlesManageViewService);
            GoToManageShopsCommand = new Commands.NavigateCommand(ShopsManageViewService);
            GoToManageCartsCommand = new Commands.NavigateCommand(CartsManageViewService);
            GoToCompareCommand = new Commands.NavigateCommand(CompareViewService);
            GoToCartsInformationCommand = new Commands.NavigateCommand(CartsInformationService);

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
