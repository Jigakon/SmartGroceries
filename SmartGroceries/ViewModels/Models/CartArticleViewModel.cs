using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.ViewModels
{
    public class CartArticleViewModel : ViewModelBase
    {
        public List<Models.ShopArticle> shopArticles
        {
            get { return cart.Shop.ShopArticles; }
            set { cart.Shop.ShopArticles = value;}
        }
        public Models.Cart cart { get; set; }
        public Models.Article article { get; set; }
        private Models.ShopArticle _selectedShopArticle;
        public Models.ShopArticle selectedShopArticle { get => _selectedShopArticle;
            set
            {
                _selectedShopArticle = value;
                OnPropertyChanged(nameof(selectedShopArticle));
                if (selectedShopArticle != null)
                {
                    article = _selectedShopArticle.Article;
                    Price = _selectedShopArticle?.GetArticleInfo(cart.Date)?.Price ?? 0;
                    Tags = new ObservableCollection<Models.Tag>(_selectedShopArticle.Article.Tags);
                    Brand = article.Brand;
                }
                else
                {
                    article.Id = Guid.NewGuid();
                }
            }
        }
        public uint Quantity { get; set; }
        private float _price;
        public float Price { get=>_price; set { _price = value; OnPropertyChanged(nameof(Price)); } }
        private ObservableCollection<Models.Tag> _tags;
        public ObservableCollection<Models.Tag> Tags { get => _tags; set { _tags = value; OnPropertyChanged(nameof(Tags)); } }
        private string _brand;
        public string Brand { get => _brand; set { _brand = value; article.Brand = _brand; OnPropertyChanged(nameof(Brand)); } }
        public CartArticleViewModel(Models.CartArticle cartArticle)
        {
            cart = cartArticle.Cart;
            article = cartArticle.Article;
            Quantity = cartArticle.Quantity;
            selectedShopArticle = cart.Shop.GetShopArticle(article.Id);
            Price = selectedShopArticle?.GetArticleInfo(cart.Date)?.Price ?? 0f;
        }
    }
}
