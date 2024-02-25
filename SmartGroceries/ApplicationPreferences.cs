using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartGroceries
{
    public class ApplicationPreferences
    {
        public string TagsPath { get; set; }
        public string ArticlesPath { get; set; }
        public string ShopsPath { get; set; }
        public string CartsPath { get; set; }
        public bool GlobalCompactMode { get; set; } = true;

        string filePath;

        public ApplicationPreferences()
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appDataFolder = Path.Combine(localAppData, "SmartGroceries");

            if (!Directory.Exists(appDataFolder))
            {
                Directory.CreateDirectory(appDataFolder);
            }

            filePath = Path.Combine(appDataFolder, "preferences.json");
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                TagsPath = Path.Combine(appDataFolder, "tags.json");
                ArticlesPath = Path.Combine(appDataFolder, "articles.json");
                ShopsPath = Path.Combine(appDataFolder, "shops.json");
                CartsPath = Path.Combine(appDataFolder, "carts.json");
                Save();
            }
            
        }

        public void Load()
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) return;
            string json = File.ReadAllText(filePath);
            var a = JsonSerializer.Deserialize<ApplicationPreferences>(json);
            TagsPath = a.TagsPath;
            ArticlesPath = a.ArticlesPath;
            ShopsPath = a.ShopsPath;
            CartsPath = a.CartsPath;
            GlobalCompactMode = a.GlobalCompactMode;

            if (!string.IsNullOrEmpty(TagsPath) && !File.Exists(TagsPath))
                using (StreamWriter sw = File.CreateText(TagsPath)) { sw.Write("[]"); }
            if (!string.IsNullOrEmpty(ArticlesPath) && !File.Exists(ArticlesPath))
                using (StreamWriter sw = File.CreateText(ArticlesPath)) { sw.Write("[]"); }
            if (!string.IsNullOrEmpty(ShopsPath) && !File.Exists(ShopsPath))
                using (StreamWriter sw = File.CreateText(ShopsPath)) { sw.Write("[]"); }
            if (!string.IsNullOrEmpty(CartsPath) && !File.Exists(CartsPath))
                using (StreamWriter sw = File.CreateText(CartsPath)) { sw.Write("[]"); }
        }

        public void Save()
        {
            var json = JsonSerializer.Serialize(this);
            if (string.IsNullOrEmpty(json)) return;

            using (StreamWriter sw = File.CreateText(filePath))
                sw.Write(json);
        }
    }
}
