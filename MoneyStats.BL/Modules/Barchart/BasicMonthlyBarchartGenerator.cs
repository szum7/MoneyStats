using MoneyStats.BL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyStats.BL.Modules.Barchart
{
    public class BasicMonthlyBarchartGenerator
    {
        public List<BasicMonthlyBar> Get(DateTime from, DateTime to)
        {
            var chart = new BasicMonthlyBarchart();
            chart.From = from;
            chart.To = to;

            var ret = chart.Bars;
            var transactions = new TransactionRepository().Get();
            transactions = transactions.OrderBy(x => x.Date);

            foreach (var tr in transactions)
            {
                if (tr.Date < chart.From || tr.Date > chart.To)
                    continue;

                var currentMonthBar = ret.Last();
                if (ret.Count == 0 || this.isSameMonth(currentMonthBar.Date, tr.Date.Value))
                {
                    currentMonthBar = new BasicMonthlyBar();
                    currentMonthBar.Date = new DateTime(tr.Date.Value.Year, tr.Date.Value.Month, 1);

                    ret.Add(currentMonthBar);
                }

                currentMonthBar.Flow += tr.Sum.Value;

                if (tr.Sum.Value < 0)
                    currentMonthBar.Expense += tr.Sum.Value;
                else
                    currentMonthBar.Income += tr.Sum.Value;

                currentMonthBar.Transactions.Add(tr);
            }

            return ret;
        }

        bool isSameMonth(DateTime d1, DateTime d2)
        {
            return d1.Month == d2.Month && d1.Year == d2.Year;
        }
    }
}
