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
        public Rule Save(Rule rule)
        {
            try
            {
                using (var context = new MoneyStatsContext())
                {
                    // Add Rule
                    rule.SetNew();
                    context.Rules.Add(rule);
                    context.SaveChanges();

                    // Add AndConditionGroup
                    foreach (var andCond in rule.AndConditionGroups)
                    {
                        andCond.SetNew();
                        andCond.RuleId = rule.Id;
                        context.AndConditionGroups.Add(andCond);
                    }
                    context.SaveChanges();

                    // Add Condition
                    foreach (var andCond in rule.AndConditionGroups)
                    {
                        foreach (var cond in andCond.Conditions)
                        {
                            cond.SetNew();
                            cond.AndConditionGroup.Id = andCond.Id;
                            context.Conditions.Add(cond);
                        }
                    }

                    // Add RuleAction
                    foreach (var action in rule.RuleActions)
                    {
                        action.SetNew();
                        action.RuleId = rule.Id;
                        action.TagId = action.Tag.Id;
                        context.RuleActions.Add(action);
                    }
                    context.SaveChanges();
                }
                return rule;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<Rule> SaveRules(List<Rule> rules)
        {
            try
            {
                using (var context = new MoneyStatsContext())
                {
                    // Add Rule
                    foreach (var rule in rules)
                    {
                        rule.SetNew();
                        context.Rules.Add(rule);
                    }
                    context.SaveChanges();

                    // Add AndConditionGroup
                    foreach (var rule in rules)
                    {
                        foreach (var andCond in rule.AndConditionGroups)
                        {                        
                            andCond.SetNew();
                            andCond.RuleId = rule.Id;
                            context.AndConditionGroups.Add(andCond);
                        }
                    }
                    context.SaveChanges();

                    foreach (var rule in rules)
                    {
                        // Add Condition
                        foreach (var andCond in rule.AndConditionGroups)
                        {
                            foreach (var cond in andCond.Conditions)
                            {
                                cond.SetNew();
                                cond.AndConditionGroup.Id = andCond.Id;
                                context.Conditions.Add(cond);
                            }
                        }

                        // Add RuleAction
                        foreach (var action in rule.RuleActions)
                        {
                            action.SetNew();
                            action.RuleId = rule.Id;
                            action.TagId = action.Tag.Id;
                            context.RuleActions.Add(action);
                        }
                    }
                    context.SaveChanges();
                }
                return rules;
            }
            catch (Exception e)
            {
                return null;
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
