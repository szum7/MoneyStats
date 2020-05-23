﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    /// <summary>
    /// Example 1: RuleGroup = (a & b & c) || (d & e) || f => x, y
    /// This class/model/table is basically the OrRuleGroup wrapped in 
    /// together with the actions and some properties (like the Title)
    /// </summary>
    [Table("RuleGroup")]
    public partial class RuleGroup : EntityBase
    {
        public string Title { get; set; }

        public virtual List<AndRuleGroup> AndRuleGroups { get; set; }
        public virtual List<RuleAction> RuleActions { get; set; }
        public virtual List<RulesetRuleGroupConn> RulesetRuleGroupConn { get; set; }
        public virtual List<TransactionCreatedWithRule> TransactionCreatedWithRule { get; set; }
    }

    public partial class RuleGroup
    {
        public RuleGroup()
        {
            this.RulesetRuleGroupConn = new List<RulesetRuleGroupConn>();
            this.TransactionCreatedWithRule = new List<TransactionCreatedWithRule>();
            this.AndRuleGroups = new List<AndRuleGroup>();
            this.RuleActions = new List<RuleAction>();
        }

        public override string ToString()
        {
            if (this.AndRuleGroups.Count == 0 || this.AndRuleGroups[0].Rules.Count == 0 || this.RuleActions.Count == 0)
                return "";

            var ruleActions = string.Join(", ", RuleActions);

            if (this.AndRuleGroups.Count == 1)
                return $"{this.AndRuleGroups[0]} => {ruleActions}";

            return $"({string.Join(") || (", AndRuleGroups)}) => {ruleActions}";
        }
    }
}