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
        public int RuleTypeId { get; set; }
        public int AndRuleGroupId { get; set; }

        public virtual RuleType RuleType { get; set; }
        public virtual AndRuleGroup AndRuleGroup { get; set; }

        public override string ToString()
        {
            switch (RuleTypeId)
            {
                case (int)RuleTypeEnum.TrueRule:
                    return "true";
                case (int)RuleTypeEnum.IsEqualTo:
                    return $"{Property} == '{Value}'";
                case (int)RuleTypeEnum.IsGreaterThan:
                    return $"{Property} > {Value}";
                case (int)RuleTypeEnum.IsLesserThan:
                    return $"{Property} < {Value}";
                case (int)RuleTypeEnum.IsPropertyNull:
                    return $"{Property} is Null";
                case (int)RuleTypeEnum.IsPropertyNotNull:
                    return $"{Property} is not Null";
                case (int)RuleTypeEnum.ContainsValueOfProperty:
                    return $"{Property}.Contains({Value})";
                default:
                    return base.ToString();
            }
        }
    }
}
