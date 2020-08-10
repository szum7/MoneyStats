using System;
using System.Collections.Generic;

namespace MoneyStats.BL.Modules.Barchart
{
    public class BasicMonthlyBarchart
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public List<BasicMonthlyBar> Bars { get; set; }

        public BasicMonthlyBarchart()
        {
            this.Bars = new List<BasicMonthlyBar>();
        }
    }
}
