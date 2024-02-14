using SmartGroceries.Commands;
using SmartGroceries.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartGroceries.ViewModels
{
    public class ShopViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        public Guid Id { get; set; }
        private string _name;
        public string Name { get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); } }
        private string _group;
        public string Group { get => _group; set { _group = value; OnPropertyChanged(nameof(Group)); } }
        private string _location;
        public string Location { get => _location; set { _location = value; OnPropertyChanged(nameof(Location)); } }
        public ObservableCollection<ShopArticleViewModel> Articles { get; set; }

        public ICommand GoToShopManageCommand { get; }

        public ShopViewModel(Models.Shop shop,
            Stores.NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            Id = shop.Id;
            Name = shop.Name;
            Group = shop.Group;
            Location = shop.Location;
            Articles = new ObservableCollection<ShopArticleViewModel>();
            foreach(var article in shop.ShopArticles)
                Articles.Add(new ShopArticleViewModel(article, navigationStore));
            GoToShopManageCommand = new Commands.NavigateCommand(new Services.NavigationService(navigationStore, CreateShopManageViewModel));
        }

        public ShopViewModel(Stores.NavigationStore navigationStore, string name = "New Article", string group = "New Group", string location = "New Address")
        {
            _navigationStore = navigationStore;
            Id = Guid.NewGuid();
            Name = name;
            Group = group;
            Location = location;
            Articles = new ObservableCollection<ShopArticleViewModel>();
            GoToShopManageCommand = new Commands.NavigateCommand(new Services.NavigationService(navigationStore, CreateShopManageViewModel));
        }

        public ViewModels.ShopManageViewModel CreateShopManageViewModel()
        {
            return new ViewModels.ShopManageViewModel(this, new Services.NavigationService(_navigationStore, CreateShopsManageViewModel));
        }

        public ViewModels.ShopsManageViewModel CreateShopsManageViewModel()
        {
            return new ViewModels.ShopsManageViewModel(_navigationStore);
        }
    }
}
