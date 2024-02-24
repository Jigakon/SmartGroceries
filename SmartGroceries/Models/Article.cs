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
        public Tag Tag { get; set; }

        public Article()
        {
            Id = Guid.Empty;
            Name = "New Article";
            Brand = "New Brand";
            Tag = null;
        }

        public Article(Article other)
        {
            Id = other.Id;
            Name = other.Name;
            Brand = other.Brand;
            Tag = other.Tag;
        }

        public Article(string name = "New Article", string brand = "New Brand", Tag tag = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Brand = brand;
            Tag = tag;
        }

        public Article(Guid id, string name = "New Article", string brand = "New Brand", Tag tag = null)
        {
            Id = id;
            Name = name;
            Brand = brand;
            Tag = tag;
        }

        public void SetTag(Tag tag)
        {
            Tag = tag;
        }

        public void RemoveTag()
        {
            Tag = null;
        }
    }
}
