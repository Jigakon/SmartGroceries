using SmartGroceries.Models;
using SmartGroceries.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Commands
{
    public class RemoveTagCommand : CommandBase
    {
        private TagViewModel _tagViewModel;

        public RemoveTagCommand(TagViewModel tagViewModel)
        {
            _tagViewModel = tagViewModel;
        }

        public override void Execute(object parameter)
        {
            switch (_tagViewModel.ViewModelContainer)
            {
                case TagsManageViewModel tagsManageViewModel:
                    tagsManageViewModel.RemoveTag(_tagViewModel);
                    break;
                case ArticleViewModel articleManageViewModel:
                    articleManageViewModel.RemoveTag(_tagViewModel);
                    break;
            }

            // temp : save in Json the changes
            GlobalDatabase.SaveTagsInJSON();
        }
    }
}
