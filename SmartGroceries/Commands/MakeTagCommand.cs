using SmartGroceries.ViewModels;
using SmartGroceries.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Commands
{
    public class MakeTagCommand : CommandBase
    {
        private readonly ViewModels.ViewModelBase _containerViewModel;

        public MakeTagCommand(ViewModels.ViewModelBase container)
        {
            _containerViewModel = container;
        }

        public override void Execute(object parameter)
        {
            switch (_containerViewModel)
            {
                case TagsManageViewModel tagManageViewModel:
                    var tagViewModel = new TagViewModel();
                    tagViewModel.ViewModelContainer = _containerViewModel;
                    tagManageViewModel.AddTag(tagViewModel);
                    break;
                case ArticleViewModel articleViewModel:
                    var window = new MakeTagWindow(_containerViewModel);
                    window.ShowDialog();
                    break;
            }
        }
    }
}
