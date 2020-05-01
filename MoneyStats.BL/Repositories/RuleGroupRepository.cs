﻿using System.Collections.Generic;
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
                    .Where(rule => rule.IsActive)
                    .Include(x => x.AndRuleGroups)
                    .Include(x => x.RuleActions)                    
                    .ToList();

                // TODO + test

                return list;
            }
        }
    }
}
