using ExpenseTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker
{
    public static class ExpenseStore
    {
        public static List<Expense> Expenses { get; set; } = new List<Expense>();

    }

}
