﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MoneyStats.BL.Interfaces;
using MoneyStats.DAL;
using MoneyStats.DAL.Models;

namespace MoneyStats.BL.Repositories
{
    public class RuleRepository : EntityBaseRepository<Rule>, IRuleRepository
    {
        public List<Rule> GetWithEntities2()
        {
            using (var context = new MoneyStatsContext())
            {

                //list = context.Rules
                //    .Where(rule => rule.State == 1)
                //    .Include(x => x.RuleActions)
                //    .Include(x => x.AndConditionGroups).ThenInclude(x => x.Conditions)
                //    .ToList();

                // Rules
                var rules = context.Rules.Where(rule => rule.State == 1).ToList();
                foreach (var rule in rules)
                {
                    // AndConditionGroups
                    rule.AndConditionGroups = context.AndConditionGroups.Where(x => x.RuleId == rule.Id).ToList();
                    foreach (var andConditionGroup in rule.AndConditionGroups)
                    {
                        // Conditions
                        andConditionGroup.Conditions = context.Conditions.Where(x => x.AndConditionGroupId == andConditionGroup.Id).ToList();
                    }

                    // RuleActions
                    rule.RuleActions = context.RuleActions.Where(x => x.RuleId == rule.Id).ToList();
                    foreach (var ruleAction in rule.RuleActions)
                    {
                        // [Tag]
                        if (ruleAction.TagId != null)
                        {
                            ruleAction.Tag = context.Tags.SingleOrDefault(x => x.Id == ruleAction.TagId);
                        }
                    }
                }

                return rules;
            }
        }

        public List<Rule> GetWithEntities()
        {
            using (var context = new MoneyStatsContext())
            {
                var list = context.Rules.Where(rule => rule.State == 1).ToList();

                return list;
            }
        }

        public List<Rule> GetWithEntitiesFaulty()
        {
            using (var context = new MoneyStatsContext())
            {
                var list = new List<Rule>();

                list = context.Rules
                    .Where(rule => rule.State == 1)
                    .Include(x => x.RuleActions)
                    .Include(x => x.AndConditionGroups).ThenInclude(x => x.Conditions)
                    .ToList();

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
