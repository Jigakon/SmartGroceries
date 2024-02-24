using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public interface ViewModelManage
    {
        void Save();

        void Remove(ViewModelData viewModel);
    }

    public class ViewModelData : ViewModelBase
    {
        public ViewModelBase ViewModelContainer { get; set; }
        public virtual void Remove(ViewModelData viewModel) { }
    }
}
