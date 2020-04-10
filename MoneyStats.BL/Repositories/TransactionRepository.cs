using MoneyStats.BL.Interfaces;
using MoneyStats.DAL;
using MoneyStats.DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace MoneyStats.BL.Repositories
{
    public class TransactionRepository : EntityBaseRepository<BankRow>, ITransactionRepository
    {
        public new int Insert(BankRow transaction)
        {
            using (var context = new MoneyStatsContext())
            {
                var transactions = this.Get();
                if (transactions.Any(x => x.ContentId == transaction.ContentId))
                {
                    return 0;
                }
                context.Transactions.Add(transaction);
                context.SaveChanges();
                return transaction.Id;
            }
        }

        public void ExtraMethod()
        {
            using (var context = new MoneyStatsContext())
            {
                var list = context.Transactions.ToList().Where(x => x.IsActive).ToList();
            }
        }

        public List<BankRow> LazyLoading()
        {
            using (var context = new MoneyStatsContext())
            {
                return context.Transactions.ToList();
            }
        }

        public List<BankRow> EagerLoading()
        {
            using (var context = new MoneyStatsContext())
            {
                return (from e in context.Transactions
                        select new BankRow()
                        {
                            Id = e.Id,
                            State = e.State,
                            TransactionTagConn = e.TransactionTagConn
                        }).ToList();
            }
        }
    }
}
