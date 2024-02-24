using SmartGroceries.Commands;
using SmartGroceries.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartGroceries.ViewModels
{
    public class ShopsManageViewModel : ViewModelBase, ViewModelManage
    {
        private readonly Stores.NavigationStore _navigationStore;

        private ObservableCollection<ShopViewModel> _shopViewModels { get; set; }
        public ObservableCollection<ShopViewModel> SearchedViewModels { get => _shopViewModels; }
        private List<Guid> RemovedShops = new List<Guid>();
        
        #region Commands
        public ICommand MakeShopCommand { get; }
        public ICommand SaveShopsCommand { get; }
        #endregion

        public ShopsManageViewModel(Stores.NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            _shopViewModels = new ObservableCollection<ShopViewModel>();

            MakeShopCommand = new Commands.MakeShopCommand(this, navigationStore);
            SaveShopsCommand = new Commands.SaveManageCommand(this);

            ResetShops();
        }

        public void ResetShops()
        {
            _shopViewModels.Clear();
            foreach (var shop in GlobalDatabase.Instance.Shops)
                _shopViewModels.Add(new ShopViewModel(shop, this, _navigationStore));
            OnPropertyChanged(nameof(SearchedViewModels));
        }

        public void AddShop(ShopViewModel shopViewModel)
        {
            _shopViewModels.Add(shopViewModel);
            OnPropertyChanged(nameof(SearchedViewModels));
            Save();
        }

        public void Save()
        {
            foreach (var shopID in RemovedShops)
                GlobalDatabase.RemoveShop(shopID);
            foreach (var shopViewModel in _shopViewModels)
                shopViewModel.Save();
        }

        public void Remove(ViewModelData viewModel)
        {
            var shopViewModel = viewModel as ShopViewModel;
            _shopViewModels.Remove(shopViewModel);
            OnPropertyChanged(nameof(SearchedViewModels));
            RemovedShops.Add(shopViewModel.Id);
        }
    }
}
