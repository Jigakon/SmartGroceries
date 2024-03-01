using SmartGroceries.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartGroceries.ViewModels
{
    public class ArticleInfoViewModel : ViewModelData
    {
        private DateTime _date;
        public DateTime Date { get => _date; 
            set
            {
                _date = value.Date;
                OnPropertyChanged(nameof(Date));
                (ViewModelContainer as ViewModels.ArticleInfosManageViewModel)?.Sort();
            }
        }
        public float _price;
        public float Price { get => _price; set { _price = value; OnPropertyChanged(nameof(Price)); OnPropertyChanged(nameof(PriceQuantity)); } }
        public float PriceQuantity 
        { 
            get => (UnitQuantity != 0f) ? Price / UnitQuantity : 0f;
            set
            {
                Price = value * UnitQuantity;
                OnPropertyChanged(nameof(Price)); 
                OnPropertyChanged(nameof(PriceQuantity));
            }
        }
        /// <summary>
        /// in what unit quantity the Article is selled : weight (kg), volume (l) or piece.
        /// Examples : Water 1l, Chicken 1.5kg... 
        /// </summary>
        private float _unitQuantity;
        public float UnitQuantity 
        { 
            get => _unitQuantity; 
            set 
            { 
                _unitQuantity = value; 
                OnPropertyChanged(nameof(UnitQuantity));
                OnPropertyChanged(nameof(PriceQuantity));
            } 
        }

        public ICommand DeleteArticleInfoCommand { get; }
        public ArticleInfoViewModel(Models.ArticleInfo articleInfo, ViewModels.ViewModelBase _viewModelContainer) 
        { 
            Date = articleInfo.Date;
            Price = articleInfo.Price;
            UnitQuantity = articleInfo.UnitQuantity;
            DeleteArticleInfoCommand = new Commands.RemoveArticleInfoCommand(this);
            ViewModelContainer = _viewModelContainer;
        }

        public ArticleInfoViewModel(ViewModels.ViewModelBase _viewModelContainer)
        {
            Date = DateTime.Today;
            Price = 0f;
            UnitQuantity = 0f;
            DeleteArticleInfoCommand = new Commands.RemoveArticleInfoCommand(this);
            ViewModelContainer = _viewModelContainer;
        }

        public Models.ArticleInfo MakeArticleInfo() 
        {
            return new Models.ArticleInfo(Price, Date, UnitQuantity);
        }
    }
}
