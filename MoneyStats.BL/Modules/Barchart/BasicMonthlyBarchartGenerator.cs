using MoneyStats.BL.Repositories;
using System;
using System.Linq;

namespace MoneyStats.BL.Modules.Barchart
{
    public class BasicMonthlyBarchartGenerator
    {
        public BasicMonthlyBarchart Get(DateTime from, DateTime to)
        {
            var chart = new BasicMonthlyBarchart();
            chart.From = from;
            chart.To = to;
            chart.MaxValue = 0;

            var transactions = new TransactionRepository().Get();
            transactions = transactions.OrderBy(x => x.Date);

            foreach (var tr in transactions)
            {
                if (tr.Date < chart.From || tr.Date > chart.To)
                    continue;

                BasicMonthlyBar currentMonthBar = null;

                if (chart.Bars.Count > 0)
                {
                    currentMonthBar = chart.Bars.Last();
                }

                if (currentMonthBar == null || !this.IsSameMonth(currentMonthBar.Date, tr.Date.Value))
                {
                    if (currentMonthBar != null && chart.MaxValue < this.GetLocalMax(currentMonthBar))
                    {
                        chart.MaxValue = this.GetLocalMax(currentMonthBar);
                    }

                    currentMonthBar = new BasicMonthlyBar();
                    currentMonthBar.Date = new DateTime(tr.Date.Value.Year, tr.Date.Value.Month, 1);

                    chart.Bars.Add(currentMonthBar);
                }

                currentMonthBar.Flow += tr.Sum.Value;

                if (tr.Sum.Value < 0)
                    currentMonthBar.Expense += tr.Sum.Value;
                else
                    currentMonthBar.Income += tr.Sum.Value;

                currentMonthBar.Transactions.Add(tr);
            }

            return chart;
        }

        decimal GetLocalMax(BasicMonthlyBar currentMonthBar) => Math.Max(currentMonthBar.Income, Math.Abs(currentMonthBar.Expense));

        bool IsSameMonth(DateTime d1, DateTime d2) => d1.Month == d2.Month && d1.Year == d2.Year;
    }
}
