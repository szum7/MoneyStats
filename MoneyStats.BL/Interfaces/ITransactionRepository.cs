using MoneyStats.DAL.Models;
using System.Collections.Generic;

namespace MoneyStats.BL.Interfaces
{
    public interface ITransactionRepository : IEntityBaseRepository<Transaction>
    {
        List<string> GetTransactionRulableProperties();
    }
}
