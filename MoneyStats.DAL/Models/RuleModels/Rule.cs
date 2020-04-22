using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoneyStats.DAL.Models
{
    /// <summary>
    /// User created.
    /// </summary>
    [Table("Rule")]
    public class Rule : EntityBase
    {
        public RuleType Type { get; set; }
        public string Property { get; set; }
        public object Value { get; set; }

        public int AndRuleGroupId { get; set; }
    }
}
