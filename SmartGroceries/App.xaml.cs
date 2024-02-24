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
using System.Windows.Media;

namespace SmartGroceries
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string GetColorAsString(string key) => (App.Current.FindResource("AccentColor") as SolidColorBrush)?.Color.ToString() ?? "FFFFFF";

        private readonly NavigationStore _navigationStore;
        public App()
        {
            _navigationStore = new NavigationStore();
        }
        public ApplicationPreferences preferences;
        protected override void OnStartup(StartupEventArgs e)
        {
            string _projectDir = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName ?? string.Empty;

            preferences = new ApplicationPreferences();
            preferences.Load();

            GlobalDatabase.SetTagPath(preferences.TagsPath);
            GlobalDatabase.SetArticlePath(preferences.ArticlesPath);
            GlobalDatabase.SetShopPath(preferences.ShopsPath);
            GlobalDatabase.SetCartPath(preferences.CartsPath);

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
                    new Services.NavigationService(_navigationStore, CreateCartsManageViewModel),
                    new Services.NavigationService(_navigationStore, CreateCompareViewModel),
                    new Services.NavigationService(_navigationStore, CreateCartsInformationsViewModel))
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
        private CompareShopArticleViewModel CreateCompareViewModel()
        {
            return new CompareShopArticleViewModel();
        }

        private CartInfomationsViewModel CreateCartsInformationsViewModel()
        {
            return new CartInfomationsViewModel();
        }

        protected override void OnExit(System.Windows.ExitEventArgs e)
        {
            GlobalDatabase.SaveTagsInJSON();
            GlobalDatabase.SaveArticlesInJSON();
            GlobalDatabase.SaveShopsInJSON();
            GlobalDatabase.SaveCartsInJSON();
        }

        public bool IsAmbientPropertyAvailable(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
