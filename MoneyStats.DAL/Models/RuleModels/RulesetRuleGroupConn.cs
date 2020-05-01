using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    [Table("RulesetRuleGroupConn")]
    public class RulesetRuleGroupConn : EntityBase
    {
        [ForeignKey("Ruleset")]
        public int RulesetId { get; set; }

        [ForeignKey("RuleGroup")]
        public int RuleGroupId { get; set; }

        public virtual RuleGroup RuleGroup { get; set; }

        public virtual Ruleset Ruleset { get; set; }
    }
}
