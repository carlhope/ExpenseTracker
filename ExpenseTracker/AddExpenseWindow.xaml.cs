using ExpenseTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ExpenseTracker
{
    /// <summary>
    /// Interaction logic for AddExpenseWindow.xaml
    /// </summary>
    public partial class AddExpenseWindow : Window
    {
        public AddExpenseWindow()
        {
            InitializeComponent();
            DatePicker.SelectedDate = DateTime.Now;
            CategoryComboBox.ItemsSource = Enum.GetValues(typeof(ExpenseCategory));
                

        }
        private void SaveExpense_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(AmountTextBox.Text, out decimal amount))
            {
                var expense = new Expense
                {
                    Description = DescriptionTextBox.Text,
                    Category = (ExpenseCategory)CategoryComboBox.SelectedItem,
                    Amount = amount,
                    Date = DatePicker.SelectedDate ?? DateTime.Now
                };

                ExpenseStore.Expenses.Add(expense);
                MessageBox.Show("Expense added!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter a valid amount.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
