using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Models
{
    public class Article
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public List<Tag> Tags { get; set; }

        public Article()
        {
            Id = Guid.Empty;
            Name = "New Article";
            Brand = "New Brand";
            Tags = new List<Tag>();
        }

        public Article(string name = "New Article", string brand = "New Brand", List<Tag> tags = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Brand = brand;
            Tags = tags ?? new List<Tag>();
        }

        public Article(Guid id, string name = "New Article", string brand = "New Brand", List<Tag> tags = null)
        {
            Id = id;
            Name = name;
            Brand = brand;
            Tags = tags ?? new List<Tag>();
        }
    }
}
