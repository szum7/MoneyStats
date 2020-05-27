using MoneyStats.BL.Interfaces;
using MoneyStats.DAL;
using MoneyStats.DAL.Models;
using System.Collections.Generic;

namespace MoneyStats.BL.Repositories
{
    public class TransactionCreatedWithRuleRepository : EntityBaseRepository<TransactionCreatedWithRule>, ITransactionCreatedWithRule
    {
    }
}
