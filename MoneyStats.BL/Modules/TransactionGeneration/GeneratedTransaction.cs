using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;

namespace MoneyStats.BL.Modules.TransactionGeneration
{
    public class GeneratedTransaction
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Sum { get; set; }

        public bool IsCustom { get; set; }

        public BankRow BankRowReference { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Rule> AppliedRules { get; set; }
        public List<BankRow> AggregatedBankRowReferences { get; set; }

        public GeneratedTransaction()
        {
            this.IsCustom = false;

            this.Tags = new List<Tag>();
            this.AppliedRules = new List<Rule>();
            this.AggregatedBankRowReferences = new List<BankRow>();
        }
    }
}
