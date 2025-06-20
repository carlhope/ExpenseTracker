using ExpenseTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace ExpenseTracker.ViewModels
{
    public class AddExpenseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<ExpenseCategory> Categories { get; } = new ObservableCollection<ExpenseCategory>((ExpenseCategory[])Enum.GetValues(typeof(ExpenseCategory)));

        private string _description;
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }

        private ExpenseCategory _selectedCategory;
        public ExpenseCategory SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }

        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }

        private DateTime _date = DateTime.Now;
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }


        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public void SaveExpense()
        {
            if (string.IsNullOrWhiteSpace(Description) || Amount <= 0)
            {
                // Optional: You could expose a validation message property here
                return;
            }

            var expense = new Expense
            {
                Description = Description.Trim(),
                Category = SelectedCategory,
                Amount = Amount,
                Date = Date
            };

            ExpenseStore.Expenses.Add(expense);
        }
    }
}
