using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    // (a & b & c) || (d & e) || f => x, y
    [Table("RuleGroup")]
    public class RuleGroup : EntityBase
    {
        public string Title { get; set; }

        [NotMapped]
        public List<OrRuleGroup> OrRuleGroups { get; set; }

        [NotMapped]
        public List<RuleAction> RuleActions { get; set; }
    }
}