using SmartGroceries.Models;
using SmartGroceries.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Commands
{
    public class SaveTagsCommand : CommandBase
    {
        private TagsManageViewModel _tasManageViewModel;

        public SaveTagsCommand(TagsManageViewModel tagsManageViewModel)
        {
            _tasManageViewModel = tagsManageViewModel;
        }

        public override void Execute(object parameter)
        {
            _tasManageViewModel.Save();

            // temp : save in Json the changes
            GlobalDatabase.SaveTagsInJSON();
        }
    }
}
