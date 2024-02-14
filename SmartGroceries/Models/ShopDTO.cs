using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Models
{
    public class ShopDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public string Location { get; set; }
        public List<ShopArticleDTO> ShopArticleDTO { get; set; }

        public ShopDTO()
        {
            Id = Guid.Empty;
            Name = "Debug::ShopDTO::Name";
            Group = "Debug::ShopDTO::Group";
            Location = "Debug::ShopDTO::Location";
            ShopArticleDTO = new List<ShopArticleDTO>();
        }

        public ShopDTO(Shop shop)
        {
            Id = shop.Id;
            Name = shop.Name;
            Group = shop.Group;
            Location = shop.Location;
            ShopArticleDTO = new List<ShopArticleDTO>();
            foreach(var shopArticle in  shop.ShopArticles)
                ShopArticleDTO.Add(new ShopArticleDTO(shopArticle));
        }

        public Shop MakeShop()
        {
            Shop shop = GlobalDatabase.GetShop(Id);
            if (shop == null)
            {
                shop = new Shop(Id, Name, Group, Location);
                foreach (var shopArticleDTO in ShopArticleDTO)
                    shop.TryAddArticle(shopArticleDTO.MakeShopArticle(shop));
            }
            return shop;
        }
    }
}
