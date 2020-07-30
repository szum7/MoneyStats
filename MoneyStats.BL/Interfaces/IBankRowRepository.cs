using MoneyStats.DAL.Models;
using System.Collections.Generic;

namespace MoneyStats.BL.Interfaces
{
    public interface IBankRowRepository : IEntityBaseRepository<BankRow>
    {
        List<string> GetBankRowProperties();
    }
}
