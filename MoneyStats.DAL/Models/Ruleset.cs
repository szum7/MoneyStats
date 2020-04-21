using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoneyStats.DAL.Models
{
    [Table("Ruleset")]
    public class Ruleset : EntityBase
    {
        public string Title { get; set; }
        public int RuleGroupId { get; set; }

        public virtual RuleGroup RuleGroup { get; set; }
    }
}
