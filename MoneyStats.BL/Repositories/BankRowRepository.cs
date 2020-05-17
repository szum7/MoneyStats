using MoneyStats.BL.Interfaces;
using MoneyStats.DAL;
using MoneyStats.DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace MoneyStats.BL.Repositories
{
    public class BankRowRepository : EntityBaseRepository<BankRow>, IBankRowRepository
    {
        public List<BankRow> GetWithEntities()
        {
            using (var context = new MoneyStatsContext())
            {
                var list = (from d in context.BankRows.ToList()
                            join a in context.Transactions on d.GroupedTransactionId equals a.Id
                            where d.IsActive && d.GroupedTransactionId != null
                            select new BankRow()
                            {
                                GroupedTransaction = a
                            }).ToList();

                return list;
            }
        }
    }
}
