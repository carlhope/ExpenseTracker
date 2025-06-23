using ExpenseTracker.Models;
using ExpenseTracker.ViewModels;
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
        private AddExpenseViewModel ViewModel => (AddExpenseViewModel)DataContext;

        public AddExpenseWindow()
        {
            InitializeComponent();
            DataContext = new AddExpenseViewModel();
        }

        private void SaveExpense_Click(object sender, RoutedEventArgs e)
        {
           ViewModel.HasAttemptedSubmit = true;
            BindingOperations.GetBindingExpression(DescriptionTextBox, TextBox.TextProperty)?.UpdateSource();
            BindingOperations.GetBindingExpression(AmountTextBox, TextBox.TextProperty)?.UpdateSource();


            if (ViewModel.SaveExpense())
            {

                MessageBox.Show("Expense saved!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Please fill in all fields correctly.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
