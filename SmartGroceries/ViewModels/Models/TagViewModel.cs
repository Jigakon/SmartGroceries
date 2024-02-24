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
    public class TagViewModel : ViewModelData
    {
        public Guid Id { get; set; }
        private string _name; 
        public string Name { get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); } }
        private string _color;
        public string Color { get => _color;
            set { _color = value; OnPropertyChanged(nameof(Color)); } }

        public ICommand DeleteTagCommand { get; }
        public ICommand EditTagCommand { get; }

        public TagViewModel(Models.Tag tag)
        {
            if(tag == null) tag = Tag.Empty;
            Id = tag.Id; 
            Name = tag.Name; 
            Color = tag.Color;
            DeleteTagCommand = new Commands.RemoveTagCommand(this);
            EditTagCommand = new Commands.EditTagCommand(this);
        }
        public TagViewModel(Guid id, string name = "New Tag", string color = "#FF0000FF")
        {
            Id = id; 
            Name = name; 
            Color = color;
            DeleteTagCommand = new Commands.RemoveTagCommand(this);
            EditTagCommand = new Commands.EditTagCommand(this);
        }
        public TagViewModel(string name = "New Tag", string color = "#FF0000FF")
        {
            Id = Guid.NewGuid(); 
            Name = name; 
            Color = color;
            DeleteTagCommand = new Commands.RemoveTagCommand(this);
            EditTagCommand = new Commands.EditTagCommand(this);
        }

        /// <summary>
        /// Create a new Tag from datas
        /// </summary>
        /// <returns></returns>
        public Tag MakeTag() => new Tag(Id, Name, Color);

        public void Save()
        {
            GlobalDatabase.ModifyOrAddTag(MakeTag());
        }
    }
}
