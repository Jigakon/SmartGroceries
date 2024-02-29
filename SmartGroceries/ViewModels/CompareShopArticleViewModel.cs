using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using SmartGroceries.Commands;
using SmartGroceries.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace SmartGroceries.ViewModels
{
    internal class CompareShopArticleViewModel : ViewModelBase
    {
        #region Chart
        public Func<double, string> FormatterDate { get; set; }
        private double _minDate = 0, _maxDate = 1;
        public double MinDate { get => _minDate; set { _minDate = value; OnPropertyChanged(nameof(MinDate)); } }
        public double MaxDate { get => _maxDate; set { _maxDate = value; OnPropertyChanged(nameof(MaxDate)); } }
        private Queue<Color> colors = new Queue<Color> { };
        private List<Unit> units = new List<Unit> { Unit.Weight, Unit.Weight, Unit.Weight };
        public SeriesCollection _prices;
        public SeriesCollection Prices
        {
            get => _prices;
            set
            {
                _prices = value;
                OnPropertyChanged(nameof(Prices));
            }
        }
        public LineSeries CreateLineSerie(string Name)
        {
            Color FillColor = colors.Dequeue();
            colors.Enqueue(FillColor);
            return CreateLineSerie(Name, FillColor);
        }
        public LineSeries CreateLineSerie(string Name, Color color)
        {
            Color FillColor = color;
            FillColor.A = 50;
            return new LineSeries
            {
                Values = new ChartValues<ArticleInfo>(),
                Title = "Article | Brand | Shop",
                LabelPoint = point => point.Y.ToString("0.00") + " €",
                Fill = new SolidColorBrush(FillColor),
                Stroke = new SolidColorBrush(color),
                Uid = Name
            };
        }

        internal void UpdatePrices()
        { // Get max and min date to set zoom correctly
            List<DateTime> dates = new List<DateTime>();
            foreach (var prices in Prices)
            {
                int count = prices.Values.Count;
                if (count > 0)
                {
                    dates.Add((prices.Values[0] as ArticleInfo).Date);
                    if (count > 1)
                        dates.Add((prices.Values[count - 1] as ArticleInfo).Date);
                }
            }
            if (dates.Count == 0)
                return;
            else if (dates.Count == 1)
                dates.Add(dates[0].AddDays(1));
            else
                dates.Sort();
            MinDate = dates[0].Date.Ticks / TimeSpan.FromHours(1).Ticks;
            MaxDate = dates[dates.Count - 1].Ticks / TimeSpan.FromHours(1).Ticks;
        }

        internal void ChangeUnit(int v, Unit unit)
        {
            units[v] = unit;
        }



        internal void ChangePrices(int v, List<ArticleInfo> articleInfos)
        {
            // set Prices To a chart
            if (v > Prices.Count || articleInfos == null)
                return;

            Prices[v].Values.Clear();
            Prices[v].Values.AddRange(articleInfos);

            UpdatePrices();
        }

        internal void ChangeName(int v, string name)
        {
            if (v > Prices.Count)
                return;
            string temp = name ?? "Error";
            (Prices[v] as LineSeries).Title = temp;
        }

        internal void ClearPrices(int v)
        {
            // set Prices To a chart
            if (v > Prices.Count)
                return;
            Prices[v].Values.Clear();

            UpdatePrices();
        }
        #endregion

        private bool _isPricedByUnit = false;
        public bool IsPricedByUnit
        {
            get => _isPricedByUnit;
            set
            {
                _isPricedByUnit = value;
                if (value)
                {
                    Prices.Configuration = LiveCharts.Configurations.Mappers.Xy<ArticleInfo>()
                                                    .X(dayModel => (double)dayModel.Date.Ticks / TimeSpan.FromHours(1).Ticks)
                                                    .Y(dayModel => dayModel.Price / dayModel.UnitQuantity);
                }
                else
                {
                    Prices.Configuration = LiveCharts.Configurations.Mappers.Xy<ArticleInfo>()
                                                    .X(dayModel => (double)dayModel.Date.Ticks / TimeSpan.FromHours(1).Ticks)
                                                    .Y(dayModel => dayModel.Price);
                }

                // Update Prices
                for (int i = 0; i < Prices.Count(); i++)
                {
                    var values = (Prices[i].Values as ChartValues<ArticleInfo>).ToList();
                    var unit = value ? $" €/{Helpers.UnitToString(units[i])}" : " €";
                    Prices[i].LabelPoint = point => point.Y.ToString("0.00") + unit;
                    ChangePrices(i, values);
                }
            }
        }

        public CompareShopArticleViewModel()
        {
            colors.Enqueue(Color.FromArgb(255, 114, 181, 255));
            colors.Enqueue(Color.FromArgb(255, 255, 179, 186));
            colors.Enqueue(Color.FromArgb(255, 161, 198, 152));
            colors.Enqueue(Color.FromArgb(255, 186, 255, 201));
            colors.Enqueue(Color.FromArgb(255, 255, 223, 186));

            FormatterDate = value => new System.DateTime((long)(value * TimeSpan.FromHours(1).Ticks)).ToString("yyyy-MM-dd");

            var dayConfig = LiveCharts.Configurations.Mappers.Xy<ArticleInfo>()
               .X(dayModel => (double)dayModel.Date.Ticks / TimeSpan.FromHours(1).Ticks)
               .Y(dayModel => dayModel.Price);

            Prices = new SeriesCollection(dayConfig)
            {
                CreateLineSerie("un"),
                CreateLineSerie("deux"),
                CreateLineSerie("trois")
            };


        }
    }
}
