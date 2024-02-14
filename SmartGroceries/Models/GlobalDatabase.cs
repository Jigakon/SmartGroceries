using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace SmartGroceries.Models
{
    public class GlobalDatabase
    {
        private static GlobalDatabase instance;
        public static GlobalDatabase Instance { 
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

        private string _pathToTags = string.Empty;

        public static bool SetTagPath(string newPath)
        {
            if (!PathExists(newPath)) return false;
            Instance._pathToTags = newPath;
            return true;
        }

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

        public static Tag TryGetTag(Guid tagID)
        {
            Instance._tags.TryGetValue(tagID, out Tag tag);
            return tag;
        }

        public static Tag TryGetTag(string name, string color)
        {
            return Instance._tags.FirstOrDefault(x => x.Value.Name == name && x.Value.Color == color).Value;
        }

        public static void TryAddTag(Tag tag)
        {
            Instance._tags.TryGetValue(tag.Id, out var searchedTag);
            if (searchedTag == null)
                instance._tags.Add(tag.Id, tag);
        }

        public static void TryAddTags(List<Tag> tags)
        {
            if (!PathExists(Instance._pathToTags)) return;

            foreach (var tag in tags)
                TryAddTag(tag);
        }

        public static bool SaveTags(List<Tag> tags)
        {
            if (!PathExists(Instance._pathToTags)) return false;

            instance._tags.Clear();
            foreach (var tag in tags)
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

        #region Articles
        private Dictionary<Guid, Article> _articles;
        public IEnumerable<Article> Articles => _articles.Values.ToList();

        private string _pathToArticles = string.Empty;

        public static bool SetArticlePath(string newPath)
        {
            if (!PathExists(newPath)) return false;
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

        public static bool SaveArticles(List<Article> articles)
        {
            if (!PathExists(Instance._pathToArticles)) return false;

            instance._articles.Clear();
            foreach (var article in articles)
                instance._articles.Add(article.Id, article);

            return true;
        }

        public static bool SaveArticle(Article article)
        {
            if (!PathExists(Instance._pathToArticles)) return false;

            instance._articles.TryGetValue(article.Id, out Article searchedArticle);
            if (searchedArticle == null)
                instance._articles.Add(article.Id, article);
            else
                searchedArticle = article;

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

        public static bool HasArticle(Guid id)
        {
            return Instance._articles.ContainsKey(id);
        }

        public static Article GetArticle(Guid id)
        {
            Instance._articles.TryGetValue(id, out var article);
            return article;
        }
        public static Article GetArticle(string name, string brand)
        {
            return Instance._articles.FirstOrDefault(x => x.Value.Name == name && x.Value.Brand == brand).Value;
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

        #endregion

        #region Shops
        private Dictionary<Guid, Shop> _shops;
        public IEnumerable<Shop> Shops => _shops.Values.ToList();

        private string _pathToShops = string.Empty;

        public static bool SetShopPath(string newPath)
        {
            if (!PathExists(newPath)) return false;
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

        public static bool SaveShops(List<Shop> shops)
        {
            if (!PathExists(Instance._pathToShops)) return false;

            instance._shops.Clear();
            foreach (var shop in shops)
                instance._shops.Add(shop.Id, shop);

            return true;
        }

        public static bool SaveShop(Shop shop)
        {
            if (!PathExists(Instance._pathToShops)) return false;

            instance._shops.TryGetValue(shop.Id, out Shop searchedShop);
            if (searchedShop != null)
            {
                searchedShop.Name = shop.Name;
                searchedShop.Group = shop.Group;
                searchedShop.Location = shop.Location;
                searchedShop.ShopArticles = shop.ShopArticles;
            }
            else
            {
                instance._shops.Add(shop.Id, shop);
            }
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

        public static void AddShop(Shop shop)
        {
            Instance._shops.TryGetValue(shop.Id, out Shop searchedShop);
            if (searchedShop == null)
                Instance._shops.Add(shop.Id, shop);
        }

        public static Shop GetShop(Guid id)
        {
            Instance._shops.TryGetValue(id, out var shop);
            return shop;
        }
        #endregion

        #region Carts
        public Dictionary<Guid, Cart> _carts;
        public IEnumerable<Cart> Carts => _carts.Values.ToList();

        private string _pathToCarts = string.Empty;

        public static bool SetCartPath(string newPath)
        {
            if (!PathExists(newPath)) return false;
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

        public static bool SaveCarts(List<Cart> carts)
        {
            if (!PathExists(Instance._pathToCarts)) return false;

            instance._carts.Clear();
            foreach (var cart in carts)
                instance._carts.Add(cart.Id, cart);

            return true;
        }

        public static bool SaveCart(Cart cart)
        {
            /*
            foreach(CartArticle cartArticle in cart.CartArticles)
            {
                TAGS
                foreach(Tag tag in cartArticle.Tag)
                    if (tag doesn't exists)
                        save tag in database

                ARTICLES
                if (does the article not exists)
                    save this article in database

                ARTICLE IN SHOP
                if (is article not registered in shop)
                    shop.registerArticle(article)

                ARTICLEINFO IN SHOPARTICLE
                if (is articleInfo exists in shop)
                    shop.AddInfo(article.id, articleInfo)
                
            }
            SHOP
            if (does the shop not exists)
                save this new shop in database

            SAVE CART
            */
            if (!PathExists(Instance._pathToShops)) return false;

            foreach (var cartArticle in cart.CartArticles)
            {
                TryAddTags(cartArticle.Article.Tags);
                SaveArticle(cartArticle.Article);
                cart.Shop.TryAddArticle(cartArticle.Article);
                cart.Shop.AddArticleInfo(cartArticle.Article.Id, new ArticleInfo(cartArticle.Price, cart.Date));
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
        #endregion
    }
}
