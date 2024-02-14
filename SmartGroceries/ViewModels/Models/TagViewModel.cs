using SmartGroceries.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace SmartGroceries.ViewModels
{
    public class TagViewModel : ViewModelBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }

        public ViewModels.ViewModelBase ViewModelContainer { get; set; }
        public ICommand DeleteTagCommand { get; }

        public TagViewModel(Models.Tag tag)
        {
            if(tag == null)
                tag = new Models.Tag("Invalid Tag", "#FF0000");
            Id = tag.Id; Name = tag.Name; Color = tag.Color;
            DeleteTagCommand = new Commands.RemoveTagCommand(this);
        }
        public TagViewModel(Guid id, string name = "New Tag", string color = "#FF0000FF")
        {
            Id = id; Name = name; Color = color;
            DeleteTagCommand = new Commands.RemoveTagCommand(this);
        }
        public TagViewModel(string name = "New Tag", string color = "#FF0000FF")
        {
            Id = Guid.NewGuid(); Name = name; Color = color;
            DeleteTagCommand = new Commands.RemoveTagCommand(this);
        }

        /// <summary>
        /// Create a new Tag from datas
        /// </summary>
        /// <returns></returns>
        public Tag MakeTag()
        {
            return new Tag(Id, Name, Color);
        }

        public void Save(bool Override = true)
        {
            Tag tag = GlobalDatabase.TryGetTag(Id);
            if (tag != null)
            {
                GlobalDatabase.TryAddTag(new Tag(Id, Name, Color));
            }
            else if (Override)
            {
                tag.Id = Id;
                tag.Name = Name;
                tag.Color = Color;
            }
        }
    }
}
