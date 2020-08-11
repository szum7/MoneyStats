using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;

namespace MoneyStats.BL.Modules.Barchart
{
    public class BasicMonthlyBar
    {
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
        public decimal Flow { get; set; }

        public List<Transaction> Transactions { get; set; }

        /// <summary>
        /// Day should be always 1.
        /// </summary>
        public DateTime Date { get; set; }

        public BasicMonthlyBar()
        {
            this.Transactions = new List<Transaction>();
        }
    }
}
