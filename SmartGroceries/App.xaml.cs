using SmartGroceries.Models;
using SmartGroceries.Stores;
using SmartGroceries.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SmartGroceries
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore _navigationStore;
        public App()
        {
            _navigationStore = new NavigationStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            string _projectDir = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName ?? string.Empty;

            GlobalDatabase.SetTagPath(_projectDir + @"\SmartGroceries\tags.json");
            GlobalDatabase.SetArticlePath(_projectDir + @"\SmartGroceries\articles.json");
            GlobalDatabase.SetShopPath(_projectDir + @"\SmartGroceries\shops.json");
            GlobalDatabase.SetCartPath(_projectDir + @"\SmartGroceries\carts.json");

            GlobalDatabase.LoadTagsFromJson();
            GlobalDatabase.LoadArticlesFromJson();
            GlobalDatabase.LoadShopsFromJson();
            GlobalDatabase.LoadCartsFromJson();

            _navigationStore.CurrentViewModel = new ViewModels.TagsManageViewModel();

            MainWindow = new MainWindow()
            {
                DataContext = new ViewModels.MainViewModel(_navigationStore,
                    new Services.NavigationService(_navigationStore, CreateTagsManageViewModel),
                    new Services.NavigationService(_navigationStore, CreateArticlesManageViewModel),
                    new Services.NavigationService(_navigationStore, CreateShopsManageViewModel),
                    new Services.NavigationService(_navigationStore, CreateCartsManageViewModel))
            };
            MainWindow.Show();

            base.OnStartup(e);
        }

        private TagsManageViewModel CreateTagsManageViewModel()
        {
            return new TagsManageViewModel();
        }

        private ArticlesManageViewModel CreateArticlesManageViewModel()
        {
            return new ArticlesManageViewModel();
        }

        private ShopsManageViewModel CreateShopsManageViewModel()
        {
            return new ShopsManageViewModel(_navigationStore);
        }

        private CartsManageViewModel CreateCartsManageViewModel()
        {
            return new CartsManageViewModel(_navigationStore);
        }
    }
}
