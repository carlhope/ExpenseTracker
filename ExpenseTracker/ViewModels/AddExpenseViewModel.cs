using ExpenseTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public decimal Amount { get; private set; }

        private string _amountInput;
        public string AmountInput
        {
            get => _amountInput;
            set
            {
                _amountInput = value;
                OnPropertyChanged(nameof(AmountInput));
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
        public bool SaveExpense()
        {

            if (!Regex.IsMatch(AmountInput ?? "", @"^\d+(\.\d{1,2})?$"))
                return false;

            var parsed = decimal.Parse(AmountInput, CultureInfo.InvariantCulture);
            if (parsed <= 0 || string.IsNullOrWhiteSpace(Description))
                return false;

            Amount = parsed;

            var expense = new Expense
            {
                Description = Description.Trim(),
                Category = SelectedCategory,
                Amount = Amount,
                Date = Date
            };

            ExpenseStore.Expenses.Add(expense);
            return true;
        }
    }
}
