﻿using SmartGroceries.Commands;
using SmartGroceries.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartGroceries.ViewModels
{
    public class TagsManageViewModel : ViewModelBase, ViewModelManage
    {
        private ObservableCollection<TagViewModel> _tags { get; set; }
        public ObservableCollection<TagViewModel> SearchedViewModels { get => _tags; }
        private List<Guid> RemovedTags = new List<Guid>();

        #region Commands
        public ICommand MakeTagCommand { get; }
        public ICommand SaveTagsCommand { get; }
        #endregion

        public List<Tag> Tags
        {
            get
            {
                List<Tag> tags = new List<Tag>();
                foreach (var tag in _tags)
                    tags.Add(new Tag(tag.Id, tag.Name, tag.Color));
                return tags;
            }
        }

        public TagsManageViewModel()
        {
            _tags = new ObservableCollection<TagViewModel>();

            MakeTagCommand = new Commands.MakeTagCommand(this);
            SaveTagsCommand = new Commands.SaveManageCommand(this);

            ResetTags();
        }

        public void ResetTags()
        {
            _tags.Clear();
            foreach (var tag in GlobalDatabase.Instance.Tags)
            {
                TagViewModel tagViewModel = new TagViewModel(tag);
                tagViewModel.ViewModelContainer = this;
                _tags.Add(tagViewModel);
            }
            OnPropertyChanged(nameof(SearchedViewModels));
        }

        public void AddTag(TagViewModel tagViewModel)
        {
            _tags.Add(tagViewModel);

            OnPropertyChanged(nameof(SearchedViewModels));
        }

        public void Save()
        {
            foreach(var tagID in RemovedTags)
                GlobalDatabase.RemoveTag(tagID);
            foreach (var tag in _tags)
                tag.Save();
        }

        public void Remove(ViewModelData viewModel)
        {
            var tagViewModel = viewModel as TagViewModel;
            _tags.Remove(tagViewModel);
            RemovedTags.Add(tagViewModel.Id);
            OnPropertyChanged(nameof(SearchedViewModels));
        }
    }
}
