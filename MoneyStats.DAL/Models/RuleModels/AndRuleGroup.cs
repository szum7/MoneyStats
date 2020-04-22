using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    [Table("AndRuleGroup")]
    public class AndRuleGroup : EntityBase
    {
        public int OrRuleGroupId { get; set; }

        public virtual OrRuleGroup OrRuleGroup { get; set; }

        [NotMapped]
        public List<Rule> Rules { get; set; }
    }
}