using SmartGroceries.Models;
using SmartGroceries.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Commands
{
    public class SetUnitCommand : CommandBase
    {
        ViewModelBase _ViewModel;
        Unit _unitToSet;
        public SetUnitCommand(ViewModelBase shopArticleViewModel, Unit unit)
        {
            _ViewModel = shopArticleViewModel;
            _unitToSet = unit;
        }
        public override void Execute(object parameter)
        {
            switch(_ViewModel)
            {
                case ShopArticleViewModel SAVM: SAVM.ArticleUnit = _unitToSet; break;
                case CartArticleViewModel CAVM: CAVM.ArticleUnit = _unitToSet; break;
            }
            
        }
    }
}
