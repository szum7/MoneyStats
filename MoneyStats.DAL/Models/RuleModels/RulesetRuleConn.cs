using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    [Table("RulesetRuleConn")]
    public class RulesetRuleConn : EntityBase
    {
        [ForeignKey("Ruleset")]
        public int RulesetId { get; set; }

        [ForeignKey("Rule")]
        public int RuleId { get; set; }

        public virtual Rule Rule { get; set; }

        public virtual Ruleset Ruleset { get; set; }
    }
}
