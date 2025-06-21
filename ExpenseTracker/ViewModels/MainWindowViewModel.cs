
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;


        public PlotModel MyPlotModel { get; set; }

        public decimal TotalSpent => ExpenseStore.Expenses.Sum(e => e.Amount);

        public decimal ThisMonthTotal =>
            ExpenseStore.Expenses
                .Where(e => e.Date.Month == DateTime.Now.Month && e.Date.Year == DateTime.Now.Year)
                .Sum(e => e.Amount);

        public string TopCategory =>
            ExpenseStore.Expenses
                .GroupBy(e => e.Category)
                .OrderByDescending(g => g.Sum(e => e.Amount))
                .Select(g => g.Key.ToString())
                .FirstOrDefault() ?? "N/A";

        private double _plotHeight;
        public double PlotHeight
        {
            get => _plotHeight;
            set
            {
                _plotHeight = value;
                OnPropertyChanged(nameof(PlotHeight));
            }
        }


        public MainWindowViewModel()
        {
            MyPlotModel = new PlotModel { Title = "Expenses by Category" };

            ExpenseStore.Expenses.CollectionChanged += (_, __) => UpdateDashboard();
            UpdateDashboard();
        }
        private void UpdateDashboard()
        {
            OnPropertyChanged(nameof(TotalSpent));
            OnPropertyChanged(nameof(ThisMonthTotal));
            OnPropertyChanged(nameof(TopCategory));

            var grouped = ExpenseStore.Expenses
                .GroupBy(e => e.Category)
                .Select(g => new
                    {
                        Category = g.Key.ToString(),
                        Total = g.Sum(e => e.Amount)
                    })
                .ToList();

            var categoryCount = grouped.Count;
            var barHeight = 40; // pixels per bar (adjust to taste)

            PlotHeight = (categoryCount * barHeight)+200;


            //clear old series and axes data
            MyPlotModel.Series.Clear();
            MyPlotModel.Axes.Clear();
            // Add new series data
            MyPlotModel.Series.Add(new BarSeries
            {
                ItemsSource = grouped.Select(g => new BarItem { Value = (double)g.Total }).ToList(),
                // Use the category names as labels
                LabelPlacement = LabelPlacement.Inside,
                // Format the labels to show currency
                LabelFormatString = "£{0}"
            });
            // Add new axes data
            MyPlotModel.Axes.Add(new CategoryAxis
            {
                // Position the category axis on the left side
                Position = AxisPosition.Left,
                // Set the labels to the category names
                ItemsSource = grouped.Select(g => g.Category).ToList(),
                IsZoomEnabled = false,
                IsPanEnabled = false
            });
            // Add a Logarithmic axis allowing for better visibility of small amounts


            MyPlotModel.Axes.Add(new LogarithmicAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Amount (£)",
                Base = 10,
                Minimum = 1,
                MinorStep = 1,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                IsZoomEnabled = false,
                IsPanEnabled = false
            });



            // Invalidate the plot to refresh the view

            MyPlotModel.InvalidatePlot(true);

        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
