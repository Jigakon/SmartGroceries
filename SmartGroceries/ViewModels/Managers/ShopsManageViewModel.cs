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
    public class ShopsManageViewModel : ViewModelBase
    {
        private readonly Stores.NavigationStore _navigationStore;
        private ObservableCollection<ShopViewModel> _shopViewModels { get; set; }
        public ObservableCollection<ShopViewModel> SearchedViewModels { get => _shopViewModels; }

        public ICommand MakeShopCommand { get; }
        public ICommand SaveShopsCommand { get; }

        public List<Shop> Shops
        {
            get
            {
                List<Shop> shops = new List<Shop>();
                foreach (var shopViewModel in _shopViewModels)
                {
                    Shop shop = GlobalDatabase.GetShop(shopViewModel.Id);
                    if (shop == null) 
                    { 
                        shop = new Shop(shopViewModel.Id, shopViewModel.Name, shopViewModel.Group, shopViewModel.Location);
                        foreach (var shopArticleViewModel in shopViewModel.Articles)
                            shop.TryAddArticle(new ShopArticle(shop, shopArticleViewModel.Article));
                    }
                    else
                    {
                        shop.Name = shopViewModel.Name;
                        shop.Group = shopViewModel.Group;
                        shop.Location = shopViewModel.Location;
                    }
                    shops.Add(shop);
                }
                return shops;
            }
        }

        public ShopsManageViewModel(Stores.NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            _shopViewModels = new ObservableCollection<ShopViewModel>();

            MakeShopCommand = new Commands.MakeShopCommand(this, navigationStore);
            SaveShopsCommand = new Commands.SaveShopsCommand(this);

            ResetShops();
        }

        public void ResetShops()
        {
            _shopViewModels.Clear();
            foreach (var shop in GlobalDatabase.Instance.Shops)
                _shopViewModels.Add(new ShopViewModel(shop, _navigationStore));
            OnPropertyChanged(nameof(SearchedViewModels));
        }

        public void AddShop(ShopViewModel shopViewModel)
        {
            _shopViewModels.Add(shopViewModel);
            OnPropertyChanged(nameof(SearchedViewModels));
            Save();
        }

        public void RemoveArticle(ShopViewModel shopViewModel)
        {
            _shopViewModels.Remove(shopViewModel);
            OnPropertyChanged(nameof(SearchedViewModels));
        }

        public void Save()
        {
            GlobalDatabase.SaveShops(Shops);
        }
    }
}
