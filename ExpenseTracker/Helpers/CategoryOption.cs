using ExpenseTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Helpers
{
    public class CategoryOption
    {
        public string DisplayName { get; set; }
        public ExpenseCategory? Category { get; set; }
    }
}
