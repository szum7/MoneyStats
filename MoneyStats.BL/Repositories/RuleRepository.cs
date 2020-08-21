using System;
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
        public List<Rule> Save(List<Rule> rules)
        {
            var existingRules = rules.Where(x => x.Id > 0).ToList();
            var newRules = rules.Where(x => x.Id <= 0).ToList();

            this.SaveNewRules(newRules);
            this.DeleteRange(existingRules.Select(x => x.Id));
            this.SaveNewRules(existingRules);

            return rules;
        }

        void SaveNewRules(List<Rule> rules)
        {
            if (rules.Count == 0)
                return;

            using (var context = new MoneyStatsContext())
            {
                rules.ForEach(rule =>
                {
                    if (rule.Id > 0)
                        goto _continue;

                    // Add Rule
                    rule.SetAsNew();

                    // Add AndConditionGroups
                    rule.AndConditionGroups.ForEach(andConditionGroup =>
                    {
                        andConditionGroup.SetAsNew();
                        // Add Conditions
                        andConditionGroup.Conditions.ForEach(condition => condition.SetAsNew());
                    });

                    // Add RuleActions
                    rule.RuleActions.ForEach(action =>
                    {
                        action.SetAsNew();
                        action.TagId = action.Tag?.Id;
                        action.Tag = null;
                    });

                    context.Rules.Add(rule);
                
                _continue:;

                });

                context.SaveChanges();
            }
        }

        public List<Rule> GetWithEntities()
        {
            using (var context = new MoneyStatsContext())
            {
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
