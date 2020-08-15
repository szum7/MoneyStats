using MoneyStats.BL.Common;
using MoneyStats.DAL.Models;
using System.Collections.Generic;

namespace MoneyStats.BL.Interfaces
{
    public interface IRuleRepository : IEntityBaseRepository<Rule>
    {
        Rule Save(Rule rule);

        List<Rule> SaveRules(List<Rule> rules);

        List<Rule> GetWithEntities();
    }
}
