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
    [Table("Condition")]
    public class Condition : EntityBase
    {
        public string Property { get; set; }
        public string Value { get; set; }
        public ConditionType ConditionType { get; set; }
        public int AndConditionGroupId { get; set; }

        public virtual AndConditionGroup AndConditionGroup { get; set; }

        public override string ToString()
        {
            switch (ConditionType)
            {
                case ConditionType.TrueRule:
                    return "true";
                case ConditionType.IsEqualTo:
                    return $"{Property} == '{Value}'";
                case ConditionType.IsGreaterThan:
                    return $"{Property} > {Value}";
                case ConditionType.IsLesserThan:
                    return $"{Property} < {Value}";
                case ConditionType.IsPropertyNull:
                    return $"{Property} is Null";
                case ConditionType.IsPropertyNotNull:
                    return $"{Property} is not Null";
                case ConditionType.ContainsValueOfProperty:
                    return $"{Property}.Contains('{Value}')";
                default:
                    return base.ToString();
            }
        }
    }
}
