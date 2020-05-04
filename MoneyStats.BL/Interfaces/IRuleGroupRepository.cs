using MoneyStats.DAL.Models;
using System.Collections.Generic;

namespace MoneyStats.BL.Interfaces
{
    public interface IRuleGroupRepository : IEntityBaseRepository<RuleGroup>
    {
        public List<RuleGroup> GetWithEntities();
    }
}
