using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Shapes;

namespace SmartGroceries.Models
{
    public class GlobalDatabase
    {
        private static GlobalDatabase instance;
        public static GlobalDatabase Instance
        {
            get
            {
                if (instance == null)
                    instance = new GlobalDatabase();
                return instance;
            }
        }

        public GlobalDatabase()
        {
            _tags = new Dictionary<Guid, Tag>();
            _articles = new Dictionary<Guid, Article>();
            _shops = new Dictionary<Guid, Shop>();
            _carts = new Dictionary<Guid, Cart>();
        }

        private static bool PathExists(string newPath) => !(string.IsNullOrEmpty(newPath) || !File.Exists(newPath));

        #region Tags
        private Dictionary<Guid, Tag> _tags;
        public IEnumerable<Tag> Tags => _tags.Values.ToList();

        #region TagsJSON
        private string _pathToTags = string.Empty;

        public static bool SetTagPath(string newPath)
        {
            if (!PathExists(newPath) || Instance._pathToTags == newPath) return false;
            Instance._pathToTags = newPath;
            return true;
        }
        /*
        private static void ExistsCreate(ref string path)
        {
            if (!PathExists(path))
            {
                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string appDataFolder = System.IO.Path.Combine(localAppData, "SmartGroceries");

                if (!Directory.Exists(appDataFolder))
                {
                    Directory.CreateDirectory(appDataFolder);
                }

                path = System.IO.Path.Combine(appDataFolder, "preferences.json");
            }
        }
        */
        public static bool LoadTagsFromJson()
        {
            if (!PathExists(Instance._pathToTags)) return false;

            instance._tags.Clear();
            string json = File.ReadAllText(instance._pathToTags);
            if (string.IsNullOrEmpty(json)) return false;
            var Tags = JsonSerializer.Deserialize<List<Tag>>(json);
            if (Tags == null) return false;
            foreach (var tag in Tags)
                instance._tags.Add(tag.Id, tag);
            return true;
        }

        public static bool SaveTagsInJSON()
        {
            if (!PathExists(Instance._pathToTags)) return false;

            if (instance._tags == null) return false;
            List<Tag> Tags = new List<Tag>();
            foreach (var tag in instance._tags.Values)
                Tags.Add(tag);
            string json = JsonSerializer.Serialize(Tags);
            if (string.IsNullOrEmpty(json)) return false;
            File.WriteAllText(instance._pathToTags, json);

            return true;
        }
        #endregion

        public static bool HasTag(Guid id)
        {
            return instance._tags.ContainsKey(id);
        }
        public static Tag TryGetTag(Guid tagID)
        {
            Instance._tags.TryGetValue(tagID, out Tag tag);
            return tag;
        }
        public static Tag TryGetTag(string name, string color)
        {
            return Instance._tags.FirstOrDefault(x => x.Value.Name == name && x.Value.Color == color).Value;
        }
        public static bool TryAddTag(Tag tag)
        {
            if (tag == null) return false;
            Instance._tags.TryGetValue(tag.Id, out var searchedTag);
            if (searchedTag == null)
            {
                instance._tags.Add(tag.Id, tag);
                return true;
            }
            return false;
        }
        public static void ModifyOrAddTag(Tag tag)
        {
            if (tag.Id == Guid.Empty) return;
            var searchedTag = TryGetTag(tag.Id);
            if (searchedTag != null)
            {
                searchedTag.Name = tag.Name;
                searchedTag.Color = tag.Color;
            }
            else
                TryAddTag(tag);
        }
        public static void RemoveTag(Tag tag) => RemoveTag(tag.Id);
        public static void RemoveTag(Guid tagID)
        {
            Instance._tags.Remove(tagID);
            foreach (Article article in Instance._articles.Values)
            {
                if (article.Tag == null) continue;
                if (article.Tag.Id == tagID)
                    article.RemoveTag();
            }
        }
        #endregion

        #region Articles
        private Dictionary<Guid, Article> _articles;
        public IEnumerable<Article> Articles => _articles.Values.ToList();

        #region ArticlesJSON
        private string _pathToArticles = string.Empty;
        public static bool SetArticlePath(string newPath)
        {
            if (!PathExists(newPath) || Instance._pathToArticles == newPath) return false;
            Instance._pathToArticles = newPath;
            return true;
        }

        public static bool LoadArticlesFromJson()
        {
            if (!PathExists(Instance._pathToArticles)) return false;

            // read JSON file
            instance._articles.Clear();
            string json = File.ReadAllText(instance._pathToArticles);
            if (string.IsNullOrEmpty(json)) return false;

            // read Articles DTO
            var ArticlesDTO = JsonSerializer.Deserialize<List<ArticleDTO>>(json);
            if (ArticlesDTO == null) return false;

            // convert DTO to object and push in dictionary
            foreach (var articleDTO in ArticlesDTO)
                instance._articles.Add(articleDTO.Id, articleDTO.MakeArticle());
            return true;
        }

        public static bool SaveArticlesInJSON()
        {
            if (!PathExists(Instance._pathToArticles)) return false;

            if (instance._articles == null) return false;
            // create List of DTO (reduce size in database, only register IDs of Tags)
            List<ArticleDTO> articleDTOs = new List<ArticleDTO>();
            foreach (var article in Instance._articles.Values)
                articleDTOs.Add(new ArticleDTO(article));

            // create JSON
            string json = JsonSerializer.Serialize(articleDTOs);
            if (string.IsNullOrEmpty(json)) return false;
            File.WriteAllText(instance._pathToArticles, json);

            return true;
        }
        #endregion

        public static bool SaveArticle(Article article)
        {
            instance._articles.TryGetValue(article.Id, out Article searchedArticle);
            if (searchedArticle == null)
                instance._articles.Add(article.Id, article);
            else
            {
                searchedArticle.Tag = article.Tag;
            }

            return true;
        }
        public static bool HasArticle(Guid id)
        {
            return Instance._articles.ContainsKey(id);
        }
        public static Article TryGetArticle(Guid id)
        {
            Instance._articles.TryGetValue(id, out Article article);
            return article;
        }
        public static Article TryGetArticle(string name, string brand, bool caseSensitive = false)
        {
            if (caseSensitive)
                return Instance._articles.FirstOrDefault(x => x.Value.Name == name && x.Value.Brand == brand).Value;
            string nameLower = name.ToLower(), brandLower = brand.ToLower();
            return Instance._articles.FirstOrDefault(x => x.Value.Name.ToLower() == nameLower && x.Value.Brand.ToLower() == brandLower).Value;
        }
        public static bool TryAddArticle(Article article)
        {
            if (!HasArticle(article.Id))
            {
                Instance._articles.Add(article.Id, article);
                return true;
            }
            return false;
        }
        public static void TryAddArticles(List<Article> Articles)
        {
            foreach (var Article in Articles)
                TryAddArticle(Article);
        }
        public static bool ModifyArticle(Article article)
        {
            Article searchedArticle = TryGetArticle(article.Id);
            if (searchedArticle != null)
            {
                TryAddTag(article.Tag);
                searchedArticle.Name = article.Name;
                searchedArticle.Brand = article.Brand;
                searchedArticle.Tag = article.Tag;
                return true;
            }
            return false;
        }
        public static void ModifyOrAddArticle(Article article)
        {
            if (!ModifyArticle(article))
                TryAddArticle(article);
        }
        public static void RemoveArticle(Article article) => RemoveArticle(article.Id);
        public static void RemoveArticle(Guid articleID)
        {
            Instance._articles.Remove(articleID);
            // remove article from all shops
            foreach (Shop shop in Instance._shops.Values)
                shop.RemoveArticle(articleID);
            //
        }
        #endregion

        #region Shops
        private Dictionary<Guid, Shop> _shops;
        public IEnumerable<Shop> Shops => _shops.Values.ToList();

        #region ShopsJSON
        private string _pathToShops = string.Empty;

        public static bool SetShopPath(string newPath)
        {
            if (!PathExists(newPath) || Instance._pathToShops == newPath) return false;
            Instance._pathToShops = newPath;
            return true;
        }

        public static bool LoadShopsFromJson()
        {
            if (!PathExists(Instance._pathToShops)) return false;

            instance._shops.Clear();
            string json = File.ReadAllText(instance._pathToShops);
            if (string.IsNullOrEmpty(json)) return false;

            List<ShopDTO> Shops = JsonSerializer.Deserialize<List<ShopDTO>>(json);
            if (Shops == null) return false;

            foreach (var shopDTO in Shops)
                instance._shops.Add(shopDTO.Id, shopDTO.MakeShop());
            return true;
        }

        public static bool SaveShopsInJSON()
        {
            if (!PathExists(Instance._pathToShops)) return false;

            if (instance._shops == null) return false;
            List<ShopDTO> Shops = new List<ShopDTO>();
            foreach (var shop in instance._shops.Values)
                Shops.Add(new ShopDTO(shop));
            string json = JsonSerializer.Serialize(Shops);
            if (string.IsNullOrEmpty(json)) return false;
            File.WriteAllText(instance._pathToShops, json);

            return true;
        }
        #endregion

        public static bool SaveShop(Shop shop)
        {
            instance._shops.TryGetValue(shop.Id, out Shop searchedShop);
            if (searchedShop != null)
            {
                searchedShop.Name = shop.Name;
                searchedShop.Group = shop.Group;
                searchedShop.Location = shop.Location;
                searchedShop.ShopArticles = shop.ShopArticles;
                return true;
            }
            else if (!string.IsNullOrEmpty(shop.Name))
            {
                instance._shops.Add(shop.Id, shop);
                return true;
            }
            return false;
        }
        public static bool HasShop(Guid id)
        {
            return Instance._shops.ContainsKey(id);
        }
        public static Shop TryGetShop(string ShopName, string ShopGroup = null, string ShopAddress = null)
        {
            return Instance._shops.FirstOrDefault(x => x.Value.Name == ShopName && (string.IsNullOrEmpty(ShopGroup) || x.Value.Group == ShopGroup) && (string.IsNullOrEmpty(ShopAddress) || x.Value.Location == ShopAddress)).Value;
        }

        public static Shop TryGetShop(Guid id)
        {
            Instance._shops.TryGetValue(id, out var shop);
            return shop;
        }
        private static bool TryAddShopArticle(ShopArticle shopArticle)
        {
            if (shopArticle == null) return false;
            Instance._articles.TryGetValue(shopArticle.Article.Id, out var searchedArticle);
            if (searchedArticle == null)
            {
                instance._articles.Add(shopArticle.Article.Id, shopArticle.Article);
                return true;
            }
            return false;
        }
        private static void TryAddShopArticles(List<ShopArticle> shopArticles)
        {
            foreach (var shopArticle in shopArticles)
                TryAddShopArticle(shopArticle);
        }
        public static bool TryAddShop(Shop shop)
        {
            if (!HasShop(shop.Id))
            {
                TryAddShopArticles(shop.ShopArticles.Values.ToList());
                Instance._shops.Add(shop.Id, shop);
                return true;
            }
            return false;
        }
        public static bool ModifyShop(Shop shop)
        {
            Shop searchedShop = TryGetShop(shop.Id);
            if (searchedShop != null)
            {
                TryAddShopArticles(shop.ShopArticles.Values.ToList());
                searchedShop.Name = shop.Name;
                searchedShop.Group = shop.Group;
                searchedShop.Location = shop.Location;
                return true;
            }
            return false;
        }
        public static void ModifyOrAddShop(Shop shop)
        {
            if (!ModifyShop(shop))
                TryAddShop(shop);
        }
        public static void RemoveShop(Shop shop) => RemoveArticle(shop.Id);
        public static void RemoveShop(Guid shopID)
        {
            Instance._shops.Remove(shopID);
            // delete carts with removed shop ?
        }
        #endregion

        #region Carts
        public Dictionary<Guid, Cart> _carts;
        public IEnumerable<Cart> Carts => _carts.Values.ToList();

        #region CartsJSON
        private string _pathToCarts = string.Empty;

        public static bool SetCartPath(string newPath)
        {
            if (!PathExists(newPath) || Instance._pathToCarts == newPath) return false;
            Instance._pathToCarts = newPath;
            return true;
        }

        public static bool LoadCartsFromJson()
        {
            if (!PathExists(Instance._pathToCarts)) return false;

            instance._carts.Clear();
            string json = File.ReadAllText(instance._pathToCarts);
            if (string.IsNullOrEmpty(json)) return false;

            List<CartDTO> Carts = JsonSerializer.Deserialize<List<CartDTO>>(json);
            if (Carts == null) return false;

            foreach (var cartDTO in Carts)
                instance._carts.Add(cartDTO.Id, cartDTO.MakeCart());
            return true;
        }
        public static bool SaveCartsInJSON()
        {
            if (!PathExists(Instance._pathToCarts)) return false;

            if (instance._carts == null) return false;
            List<CartDTO> Carts = new List<CartDTO>();
            foreach (var cart in instance._carts.Values)
                Carts.Add(new CartDTO(cart));
            string json = JsonSerializer.Serialize(Carts);
            if (string.IsNullOrEmpty(json)) return false;
            File.WriteAllText(instance._pathToCarts, json);

            return true;
        }
        #endregion

        public static bool SaveCart(Cart cart)
        {
            foreach (var cartArticle in cart.CartArticles)
            {
                Article article = cartArticle.Article;
                TryAddTag(article.Tag);
                SaveArticle(article);
                //cart.Shop.TryAddArticle(article);
                //cart.Shop.AddArticleInfo(article.Id, new ArticleInfo(cartArticle.Price, cart.Date, cartArticle.UnitQuantity));
            }
            SaveShop(cart.Shop);

            instance._carts.TryGetValue(cart.Id, out Cart searchedCart);
            if (searchedCart != null)
            {
                searchedCart.Id = cart.Id;
                searchedCart.Name = cart.Name;
                searchedCart.Date = cart.Date;
                searchedCart.Shop = cart.Shop;
                searchedCart.CartArticles = cart.CartArticles;
            }
            else
            {
                instance._carts.Add(cart.Id, cart);
            }
            return true;
        }

        public static void AddCart(Cart cart)
        {
            Instance._carts.TryGetValue(cart.Id, out Cart searchedCart);
            if (searchedCart == null)
                Instance._carts.Add(cart.Id, cart);
        }

        public static Cart GetCart(Guid id)
        {
            Instance._carts.TryGetValue(id, out var cart);
            return cart;
        }
        public static bool ReplaceCarts(List<Cart> carts)
        {
            instance._carts.Clear();
            foreach (var cart in carts)
                instance._carts.Add(cart.Id, cart);

            return true;
        }

        public static void RemoveCart(Guid cartID)
        {
            Instance._carts.Remove(cartID);
            // remove ArticleInfos saved in Database ?
        }

        internal static Cart TryGetCart(Guid id)
        {
            if (id == Guid.Empty) return null;
            Instance._carts.TryGetValue(id, out var cart);
            return cart;
        }

        internal static Cart TryGetCart(DateTime date, string shopName, string cartName = null)
        {
            var carts = Instance._carts.Values.Where(x => x.Date == date && (x?.Shop?.Name ?? "") == shopName).ToList();
            if (carts.Count == 1 || string.IsNullOrEmpty(cartName))
                return carts[0];
            return carts.FirstOrDefault(x => x.Name == cartName);
        }
        internal static bool TryAddCart(Cart cart)
        {
            if (!HasCart(cart.Id))
            {
                Instance._carts.Add(cart.Id, cart);
                return true;
            }
            return false;
        }

        public static bool HasCart(Guid id)
        {
            return Instance._carts.ContainsKey(id);
        }

        public static bool ModifyCart(Guid cartID, string CartName, Shop shop, DateTime date, List<CartArticle> articles = null)
        {
            Cart searchedCart = TryGetCart(cartID);
            if (searchedCart != null)
            {
                // TODO : add cart articles
                if (articles != null)
                    searchedCart.CartArticles = articles;
                searchedCart.Name = CartName;
                searchedCart.Shop = shop;
                searchedCart.Date = date;
                return true;
            }
            return false;
        }
        #endregion
    }
}
