using System.Collections.Generic;
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

        public virtual ICollection<RulesetRuleGroupConn> RulesetRuleGroupConn { get; set; }
        public virtual ICollection<TransactionCreatedWithRule> TransactionCreatedWithRule { get; set; }
    }

    public partial class RuleGroup
    {
        [NotMapped]
        public List<OrRuleGroup> OrRuleGroups { get; set; }

        [NotMapped]
        public List<RuleAction> RuleActions { get; set; }
    }
}