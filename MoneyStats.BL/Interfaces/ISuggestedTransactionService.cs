using MoneyStats.BL.Common;
using MoneyStats.BL.Modules;
using System.Collections.Generic;

namespace MoneyStats.BL.Interfaces
{
    public interface ISuggestedTransactionService
    {
        GenericResponse<bool> SaveAll(List<SuggestedTransaction> suggestedTransactions);
    }
}
