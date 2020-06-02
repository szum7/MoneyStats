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
        public string Value { get; set; }
        public RuleTypeEnum RuleType { get; set; }
        public int AndRuleGroupId { get; set; }

        public virtual AndRuleGroup AndRuleGroup { get; set; }

        public override string ToString()
        {
            switch (RuleType)
            {
                case RuleTypeEnum.TrueRule:
                    return "true";
                case RuleTypeEnum.IsEqualTo:
                    return $"{Property} == '{Value}'";
                case RuleTypeEnum.IsGreaterThan:
                    return $"{Property} > {Value}";
                case RuleTypeEnum.IsLesserThan:
                    return $"{Property} < {Value}";
                case RuleTypeEnum.IsPropertyNull:
                    return $"{Property} is Null";
                case RuleTypeEnum.IsPropertyNotNull:
                    return $"{Property} is not Null";
                case RuleTypeEnum.ContainsValueOfProperty:
                    return $"{Property}.Contains({Value})";
                default:
                    return base.ToString();
            }
        }
    }
}
