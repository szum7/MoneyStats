using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    [Table("OrRuleGroup")]
    public class OrRuleGroup : EntityBase
    {
        public int RuleGroupId { get; set; }

        public virtual RuleGroup RuleGroup { get; set; }

        [NotMapped]
        public List<AndRuleGroup> AndRuleGroups { get; set; }
    }
}