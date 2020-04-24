using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    /// <summary>
    /// Example 1: OrRuleGroup = a || b
    /// Example 2: OrRuleGroup = (a && b && c) || (d && e)
    /// Example 3: OrRuleGroup = (a && b) || c || d
    /// </summary>
    [Table("OrRuleGroup")]
    public partial class OrRuleGroup : EntityBase
    {
        public int RuleGroupId { get; set; }

        public virtual RuleGroup RuleGroup { get; set; }
    }

    public partial class OrRuleGroup
    {
        [NotMapped]
        public List<AndRuleGroup> AndRuleGroups { get; set; }
    }
}