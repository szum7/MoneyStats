using MoneyStats.BL.Modules;
using MoneyStats.DAL.Models;
using System.Collections.Generic;

namespace MoneyStats.BL.Interfaces
{
    public interface IGeneratedTransactionService
    {
        List<SuggestedTransaction> Get(List<Rule> rules, List<BankRow> bankRows);
    }
}
