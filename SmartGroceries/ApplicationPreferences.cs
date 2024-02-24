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

        public void Load()
        {
            string _projectDir = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName ?? string.Empty;
            string preferenceDir = _projectDir + @"\SmartGroceries\preferences.json";
            if (string.IsNullOrEmpty(preferenceDir) || !File.Exists(preferenceDir)) return;
            string json = File.ReadAllText(preferenceDir);
            var a = JsonSerializer.Deserialize<ApplicationPreferences>(json);
            TagsPath = a.TagsPath;
            ArticlesPath = a.ArticlesPath;
            ShopsPath = a.ShopsPath;
            CartsPath = a.CartsPath;
            GlobalCompactMode = a.GlobalCompactMode;
        }

        public void Save()
        {
            string _projectDir = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName ?? string.Empty;
            string preferenceDir = _projectDir + @"\SmartGroceries\preferences.json";

            var json = JsonSerializer.Serialize(this);
            if (string.IsNullOrEmpty(json)) return;

            using (StreamWriter sw = File.CreateText(preferenceDir))
                sw.Write(json);

        }
    }
}
