using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoneyStats.DAL.Models
{
    // (a & b & c) || (d & e) || f => x, y
    // (a & b & c) || f => y
    // f => z
    [Table("Ruleset")]
    public class Ruleset : EntityBase
    {
        public string Title { get; set; }

        public virtual ICollection<RulesetRuleGroupConn> RulesetRuleGroupConn { get; set; }
    }
}
