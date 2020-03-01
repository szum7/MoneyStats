using MoneyStats.BL.Interfaces;
using MoneyStats.DAL;
using MoneyStats.DAL.Models;
using System.Linq;

namespace MoneyStats.BL.Repositories
{
    public class TransactionRepository : EntityBaseRepository<Transaction>, ITransactionRepository
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

        public void ExtraMethod()
        {
            throw new System.NotImplementedException();
        }
    }
}
