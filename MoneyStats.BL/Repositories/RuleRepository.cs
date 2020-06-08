using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MoneyStats.BL.Interfaces;
using MoneyStats.DAL;
using MoneyStats.DAL.Models;

namespace MoneyStats.BL.Repositories
{
    public class RuleRepository : EntityBaseRepository<Rule>, IRuleRepository
    {
        public List<Rule> GetWithEntities()
        {
            using (var context = new MoneyStatsContext())
            {
                var list = new List<Rule>();

                list = context.Rules
                    .Where(rule => rule.State == 1)
                    .Include(x => x.AndConditionGroups).ThenInclude(x => x.Conditions)
                    .ToList();

                // TODO + test

                return list;
            }
        }

        public List<Rule> GetOnIdsWithEntitiesInDepth(List<int> ids)
        {
            using (var context = new MoneyStatsContext())
            {
                return context.Rules
                    .Where(x => ids.Any(y => y == x.Id) && x.State == 1)
                    .Include(x => x.RuleActions).ThenInclude(x => x.Tag)
                    .Include(x => x.AndConditionGroups).ThenInclude(x => x.Conditions)
                    .ToList();
            }
        }
    }
}
