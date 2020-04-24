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
        public int OrRuleGroupId { get; set; }

        public virtual OrRuleGroup OrRuleGroup { get; set; }
    }

    public partial class AndRuleGroup
    {
        [NotMapped]
        public List<Rule> Rules { get; set; }
    }
}