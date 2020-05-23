using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MoneyStats.BL.Interfaces;
using MoneyStats.DAL;
using MoneyStats.DAL.Models;

namespace MoneyStats.BL.Repositories
{
    public class RuleGroupRepository : EntityBaseRepository<RuleGroup>, IRuleGroupRepository
    {
        public List<RuleGroup> GetWithEntities()
        {
            using (var context = new MoneyStatsContext())
            {
                var list = new List<RuleGroup>();

                list = context.RuleGroups
                    .Where(rule => rule.State == 1)
                    .Include(x => x.RuleActions).ThenInclude(x => x.RuleActionType)
                    .Include(x => x.AndRuleGroups).ThenInclude(x => x.Rules).ThenInclude(x => x.RuleType)
                    .ToList();

                // TODO + test

                return list;
            }
        }

        public List<RuleGroup> GetOnIdsWithEntitiesInDepth(List<int> ids)
        {
            using (var context = new MoneyStatsContext())
            {
                return context.RuleGroups
                    .Where(x => ids.Any(y => y == x.Id) && x.State == 1)
                    .Include(x => x.RuleActions).ThenInclude(x => x.RuleActionType)
                    .Include(x => x.RuleActions).ThenInclude(x => x.Tag)
                    .Include(x => x.AndRuleGroups).ThenInclude(x => x.Rules).ThenInclude(x => x.RuleType)
                    .ToList();
            }
        }
    }
}
