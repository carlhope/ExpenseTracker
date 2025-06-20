using ExpenseTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.ViewModels
{
    public class HistoryViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Expense> AllExpenses { get; }
        public ObservableCollection<Expense> FilteredExpenses { get; } = new();

        private ExpenseCategory? selectedCategory;
        public ExpenseCategory? SelectedCategory
        {
            get => selectedCategory;
            set
            {
                if (selectedCategory != value)
                {
                    selectedCategory = value;
                    OnPropertyChanged(nameof(SelectedCategory));
                    ApplyFilter();
                }
            }
        }

        public IEnumerable<ExpenseCategory> Categories =>
            Enum.GetValues(typeof(ExpenseCategory)).Cast<ExpenseCategory>();

        public HistoryViewModel()
        {
            var sorted = ExpenseStore.Expenses.OrderByDescending(e => e.Date);
            AllExpenses = new ObservableCollection<Expense>(sorted);

            ExpenseStore.Expenses.CollectionChanged += (_, __) => RefreshAllExpenses();
            ApplyFilter();
        }

        private void RefreshAllExpenses()
        {
            var sorted = ExpenseStore.Expenses.OrderByDescending(e => e.Date).ToList();
            AllExpenses.Clear();
            foreach (var item in sorted)
                AllExpenses.Add(item);
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            FilteredExpenses.Clear();
            var items = selectedCategory == null
                ? AllExpenses
                : AllExpenses.Where(e => e.Category == selectedCategory);

            foreach (var expense in items)
                FilteredExpenses.Add(expense);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
