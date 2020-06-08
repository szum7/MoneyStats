using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoneyStats.DAL.Models
{
    /// <summary>
    /// Not yet in use. DO NOT USE IT only as a list of Rules!
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
        [Obsolete("Do not use Rulesets only as a list of Rules!", false)] // TODO delete when Rulesets are implemented
        public virtual List<Rule> Rules { get; set; }
        public virtual List<RulesetRuleConn> RulesetRuleConns { get; set; }
    }

    public partial class Ruleset
    {
        public Ruleset()
        {
            //this.Rules = new List<Rules>();
            this.RulesetRuleConns = new List<RulesetRuleConn>();
        }
    }
}
