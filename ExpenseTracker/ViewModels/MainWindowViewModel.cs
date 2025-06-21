using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
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
                ItemsSource = grouped.Select(g => g.Category).ToList()
            });
            // Add a linear axis for the values
            MyPlotModel.Axes.Add(new LinearAxis
            {
                // Position the linear axis on the bottom side
                Position = AxisPosition.Bottom,
                // Set Minimum padding to zero
                MinimumPadding = 0,
                // Remove space before axis start at zero
                AbsoluteMinimum = 0
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
