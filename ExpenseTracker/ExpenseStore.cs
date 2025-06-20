using ExpenseTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker
{
    public static class ExpenseStore
    {
        public static ObservableCollection<Expense> Expenses { get;} = new ObservableCollection<Expense>();

    }

}
