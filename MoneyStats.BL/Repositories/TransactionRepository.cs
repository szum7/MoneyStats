using MoneyStats.DAL;
using MoneyStats.DAL.Models;
using System.Linq;

namespace MoneyStats.BL.Repositories
{
    public class TransactionRepository : EntityBaseRepository<Transaction>
    {
        public new int Insert(Transaction transaction)
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
    }
}
