using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MoneyStats.DAL.Models
{
    /// <summary>
    /// RuleActions are not stored to be used multiple types. 
    /// They belong to exactly one Rule.
    /// </summary>
    [Table("RuleAction")]
    public partial class RuleAction : EntityBase
    {
        public string Title { get; set; }

        public RuleActionType RuleActionType { get; set; }

        public string Property { get; set; }

        public string Value { get; set; }

        [ForeignKey("Rule")]
        public int RuleId { get; set; }

        /// <summary>
        /// For AddTag RuleAction type.
        /// </summary>
        [ForeignKey("Tag")]
        public int? TagId { get; set; }


        public virtual Rule Rule { get; set; }

        public virtual Tag Tag { get; set; }


        public override string ToString()
        {
            switch (RuleActionType)
            {
                case RuleActionType.Omit:
                    return $"Omit";
                case RuleActionType.AddTag:
                    return (Tag == null) ? $"AddTag({TagId})" : $"AddTag({Tag.Id}, '{Tag.Title}')";
                case RuleActionType.SetValueOfProperty:
                    return $"{Property} = '{Value}'";
                case RuleActionType.AggregateToMonthlyTransaction:
                    return $"GroupToMonth";
                default:
                    return base.ToString();
            }
        }
    }
}