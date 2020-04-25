using MoneyStats.BL.Interfaces;
using MoneyStats.DAL;
using MoneyStats.DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace MoneyStats.BL.Repositories
{
    public class BankRowRepository : EntityBaseRepository<BankRow>, IBankRowRepository
    {
        public new int Insert(BankRow transaction)
        {
            using (var context = new MoneyStatsContext())
            {
                var transactions = this.Get();
                if (transactions.Any(x => x.ContentId == transaction.ContentId)) // TODO not-bank-specific
                {
                    return 0;
                }
                context.BankRows.Add(transaction);
                context.SaveChanges();
                return transaction.Id;
            }
        }
    }
}
