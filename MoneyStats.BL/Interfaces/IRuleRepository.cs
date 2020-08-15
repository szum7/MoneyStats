using MoneyStats.BL.Common;
using MoneyStats.DAL.Models;
using System.Collections.Generic;

namespace MoneyStats.BL.Interfaces
{
    public interface IRuleRepository : IEntityBaseRepository<Rule>
    {
        List<Rule> Save(List<Rule> rules);

        List<Rule> GetWithEntities();
    }
}
