using MoneyStats.BL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyStats.BL.Modules.Barchart
{
    public class BasicMonthlyBarchartGenerator
    {
        public BasicMonthlyBarchart Get(DateTime from, DateTime to)
        {
            // Format dates
            from = new DateTime(from.Year, from.Month, 1);
            to = new DateTime(to.Year, to.Month + 1, 1).AddDays(-1);

            // Init return value
            var chart = new BasicMonthlyBarchart();
            chart.From = from;
            chart.To = to;
            chart.MaxValue = 0;

            // Get transactions
            var transactions = new TransactionRepository().Get().ToList();

            if (transactions.Count == 0)
                return chart;

            // Order transactions
            transactions = transactions.OrderBy(x => x.Date).ToList();

            // Fill up starting dates (if needed)
            this.FillInDateGaps(chart.Bars, from, transactions.First().Date.Value.AddMonths(-1));

            // TODO !!! fix some kind of synch problem (?) 

            foreach (var tr in transactions)
            {
                if (tr.Date < chart.From || tr.Date > chart.To)
                    continue;

                BasicMonthlyBar currentMonthBar = null;

                if (chart.Bars.Count > 0)
                {
                    currentMonthBar = GetLast(chart.Bars);
                }

                if (currentMonthBar == null || !this.IsSameMonth(currentMonthBar.Date, tr.Date.Value))
                {
                    // Check for last item's localmax
                    var localMax = this.GetLocalMax(currentMonthBar);
                    if (chart.MaxValue < localMax)
                    {
                        chart.MaxValue = localMax;
                    }

                    // Fill in date gaps
                    if (currentMonthBar != null) // validated with !IsSameMonth
                    {
                        this.FillInDateGaps(chart.Bars, currentMonthBar.Date.AddMonths(1), tr.Date.Value.AddMonths(-1));
                    }

                    // New item
                    currentMonthBar = new BasicMonthlyBar();
                    currentMonthBar.Date = new DateTime(tr.Date.Value.Year, tr.Date.Value.Month, 1);

                    // Add
                    chart.Bars.Add(currentMonthBar);
                }

                currentMonthBar.Flow += tr.Sum.Value;

                if (tr.Sum.Value < 0)
                    currentMonthBar.Expense += tr.Sum.Value;
                else
                    currentMonthBar.Income += tr.Sum.Value;

                currentMonthBar.Transactions.Add(tr);
            }

            // Fill up ending dates (if needed)
            this.FillInDateGaps(chart.Bars, GetLast(chart.Bars).Date.AddMonths(1), to);

            return chart;
        }

        BasicMonthlyBar GetLast(List<BasicMonthlyBar> list) => list[list.Count - 1];

        void FillInDateGaps(List<BasicMonthlyBar> list, DateTime startDate, DateTime goalDate)
        {
            while (startDate < goalDate)
            {
                list.Add(new BasicMonthlyBar()
                {
                    Date = startDate,
                    Expense = 0,
                    Flow = 0,
                    Income = 0,
                    IsMissingMonth = true
                });
                startDate = startDate.AddMonths(1);
            }
        }

        decimal GetLocalMax(BasicMonthlyBar currentMonthBar) => 
            currentMonthBar != null ? 
            Math.Max(currentMonthBar.Income, Math.Abs(currentMonthBar.Expense)) : 
            0;

        bool IsSameMonth(DateTime d1, DateTime d2) => d1.Month == d2.Month && d1.Year == d2.Year;
    }
}
