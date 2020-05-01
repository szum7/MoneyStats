using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    /// <summary>
    /// Example 1: AndRuleGroup = a
    /// Example 2: AndRuleGroup = a && b && c
    /// Example 3: AndRuleGroup = a && b
    /// </summary>
    [Table("AndRuleGroup")]
    public partial class AndRuleGroup : EntityBase
    {
        public int RuleGroupId { get; set; }

        public virtual RuleGroup RuleGroup { get; set; }
        public virtual List<Rule> Rules { get; set; }
    }

    public partial class AndRuleGroup
    {
        public AndRuleGroup()
        {
            this.Rules = new List<Rule>();
        }
    }
}