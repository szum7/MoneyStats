using MoneyStats.BL.Interfaces;
using MoneyStats.BL.Modules.Barchart;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyStats.BL.Repositories
{
    public class StatisticsService : IStatisticsService
    {
        public BasicMonthlyBarchart Get(DateTime from, DateTime to)
        {
            return new BasicMonthlyBarchartGenerator().Get(from, to);
        }
    }
}
