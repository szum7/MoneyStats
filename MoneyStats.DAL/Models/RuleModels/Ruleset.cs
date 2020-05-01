using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoneyStats.DAL.Models
{
    /// <summary>
    /// Not yet in use. DO NOT USE IT only as a list of RuleGroups!
    /// Use different rulesets for different workflows. For example 
    /// create a ruleset for K&H transactions and another for OTP.
    // [(a & b & c) || (d & e) || f] => x, y
    // [(a & b & c) || f] => y
    // [f] => z
    /// </summary>
    [Table("Ruleset")]
    public partial class Ruleset : EntityBase
    {
        public string Title { get; set; }

        [NotMapped]
        [Obsolete("Do not use Rulesets only as a list of RuleGroups!", false)] // TODO delete when Rulesets are implemented
        public virtual List<RuleGroup> RuleGroups { get; set; }
        public virtual List<RulesetRuleGroupConn> RulesetRuleGroupConn { get; set; }
    }

    public partial class Ruleset
    {
        public Ruleset()
        {
            //this.RuleGroups = new List<RuleGroup>();
            this.RulesetRuleGroupConn = new List<RulesetRuleGroupConn>();
        }
    }
}
