using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoneyStats.DAL.Models
{
    /// <summary>
    /// User created.
    /// Example 1: transaction.property == value
    /// Example 2: transaction.property.Contains(value)
    /// </summary>
    [Table("Rule")]
    public class Rule : EntityBase
    {
        public string Property { get; set; }
        public object Value { get; set; }
        public int RuleTypeId { get; set; }
        public int AndRuleGroupId { get; set; }

        public virtual RuleType RuleType { get; set; }
        public virtual AndRuleGroup AndRuleGroup { get; set; }
    }
}
