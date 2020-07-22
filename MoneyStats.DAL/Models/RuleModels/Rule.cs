using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    /// <summary>
    /// Example 1: Rule = (a & b & c) || (d & e) || f => x, y
    /// This class/model/table is basically the OrConditionGroup wrapped in 
    /// together with the actions and some properties (like the Title)
    /// </summary>
    [Table("Rule")]
    public partial class Rule : EntityBase
    {
        public string Title { get; set; }

        public virtual List<AndConditionGroup> AndConditionGroups { get; set; }
        public virtual List<RuleAction> RuleActions { get; set; }
        public virtual List<RulesetRuleConn> RulesetRuleConns { get; set; }
        public virtual List<TransactionCreatedWithRule> TransactionCreatedWithRule { get; set; }
    }

    public partial class Rule
    {
        public Rule()
        {
            this.RulesetRuleConns = new List<RulesetRuleConn>();
            this.TransactionCreatedWithRule = new List<TransactionCreatedWithRule>();
            this.AndConditionGroups = new List<AndConditionGroup>();
            this.RuleActions = new List<RuleAction>();
        }

        public string FancyName => this.ToString();

        public override string ToString()
        {
            if (this.AndConditionGroups.Count == 0 || this.AndConditionGroups[0].Conditions.Count == 0 || this.RuleActions.Count == 0)
                return "";

            var ruleActions = string.Join(", ", RuleActions);

            if (this.AndConditionGroups.Count == 1)
                return $"{this.AndConditionGroups[0]} => {ruleActions}";

            return $"({string.Join(") || (", AndConditionGroups)}) => {ruleActions}";
        }
    }
}