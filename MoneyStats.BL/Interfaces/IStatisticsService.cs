using MoneyStats.BL.Modules.Barchart;
using System;
using System.Collections.Generic;

namespace MoneyStats.BL.Interfaces
{
    public interface IStatisticsService
    {
        BasicMonthlyBarchart Get(DateTime from, DateTime to);
    }
}
