﻿using SmartGroceries.ViewModels;
using SmartGroceries.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Commands
{
    internal class MakeShopArticleCommand : CommandBase
    {
        private readonly ViewModels.ShopArticlesManageViewModel _shopManageViewModel;

        public MakeShopArticleCommand(ViewModels.ShopArticlesManageViewModel shopManageViewModel)
        {
            _shopManageViewModel = shopManageViewModel;
        }

        public override void Execute(object parameter)
        {
            new Windows.MakeShopArticleWindow(_shopManageViewModel).ShowDialog();
        }
    }
}
