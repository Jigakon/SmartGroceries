using SmartGroceries.Commands;
using SmartGroceries.Models;
using SmartGroceries.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SmartGroceries.ViewModels
{
    public class ShopViewModel : ViewModelData
    {
        private readonly NavigationStore _navigationStore;
        public Guid Id { get; set; }
        private string _name;
        public string Name { get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); } }
        private string _group;
        public string Group { get => _group; set { _group = value; OnPropertyChanged(nameof(Group)); } }
        private string _location;
        public string Location { get => _location; set { _location = value; OnPropertyChanged(nameof(Location)); } }
        public ObservableCollection<ViewModels.ShopArticleViewModel> ShopArticles { get; set; }

        public ICommand GoToShopManageCommand { get; }
        public ICommand DeleteFromShopsManageCommand { get; }

        public ShopViewModel(Models.Shop shop, ViewModels.ViewModelBase viewModelContainer, Stores.NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            Id = shop.Id;
            Name = shop.Name;
            Group = shop.Group;
            Location = shop.Location;
            ShopArticles = new ObservableCollection<ViewModels.ShopArticleViewModel>();
            foreach(var article in shop.ShopArticles.Values)
                ShopArticles.Add(new ShopArticleViewModel(article, this, navigationStore));
            ViewModelContainer = viewModelContainer;
            GoToShopManageCommand = new Commands.NavigateCommand(new Services.NavigationService(navigationStore, CreateShopManageViewModel));
            DeleteFromShopsManageCommand = new Commands.RemoveShopCommand(this);
        }

        public ShopViewModel(Stores.NavigationStore navigationStore, string name = "New Article", string group = "New Group", string location = "New Address")
        {
            _navigationStore = navigationStore;
            Id = Guid.NewGuid();
            Name = name;
            Group = group;
            Location = location;
            ShopArticles = new ObservableCollection<ViewModels.ShopArticleViewModel>();
            GoToShopManageCommand = new Commands.NavigateCommand(new Services.NavigationService(navigationStore, CreateShopManageViewModel));
        }

        public Shop MakeShop()
        {
            List<ShopArticle> shopArticles = new List<ShopArticle>();
            foreach (var shopArticleViewModel in ShopArticles)
                shopArticles.Add(shopArticleViewModel.MakeShopArticle());
            return new Shop(Id, Name, Group, Location, shopArticles);
        }

        public void Save()
        {
            Shop shop = GlobalDatabase.TryGetShop(Id);
            if (shop == null) 
                shop = MakeShop();
            else
            {
                if (Name != shop.Name && MessageBox.Show($"The Name of the shop changed !\nModify {shop.Name} by {Name} ?", "Name Changed !", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    shop.Name = Name;
                else
                    Name = shop.Name;

                if (Group != shop.Group && MessageBox.Show($"The Group of the shop changed !\nModify {shop.Group} by {Group} ?", "Group Changed !", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    shop.Group = Group;
                else
                    Group = shop.Group;

                if (Location != shop.Location && MessageBox.Show($"The Location of the shop changed !\nModify {shop.Location} by {Location} ?", "Group Changed !", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    shop.Location = Location;
                else
                    Location = shop.Location;
            }

            // save ShopArticles
            foreach(ShopArticleViewModel shopArticleViewModel in ShopArticles)
            {
                // check if shoparticle - article exists in Database
                shopArticleViewModel.Save();
            }

            GlobalDatabase.ModifyOrAddShop(shop);
        }

        public ShopArticlesManageViewModel CreateShopManageViewModel()
        {
            return new ShopArticlesManageViewModel(this, new Services.NavigationService(_navigationStore, CreateShopsManageViewModel));
        }

        public ShopsManageViewModel CreateShopsManageViewModel()
        {
            return new ShopsManageViewModel(_navigationStore);
        }
    }
}
