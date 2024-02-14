using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;

namespace SmartGroceries.Models
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
    
        public Tag()
        {
            Id = Guid.Empty;
            Name = "New Tag";
            Color = "#FF0000FF";
        }

        public Tag(string name = "New Tag", string color = "#FF0000FF")
        {
            Id = Guid.NewGuid();
            Name = name;
            Color = color;
        }

        public Tag(Guid id, string name = "New Tag", string color = "#FF0000FF")
        {
            Id = id;
            Name = name;
            Color = color;
        }
    }
}
