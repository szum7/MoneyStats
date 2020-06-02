using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MoneyStats.DAL.Models
{
    /// <summary>
    /// RuleActions are not stored to be used multiple types. They belong
    /// to exactly one RuleGroup.
    /// </summary>
    [Table("RuleAction")]
    public partial class RuleAction : EntityBase
    {
        public string Title { get; set; }

        public RuleActionTypeEnum RuleActionType { get; set; }

        public string Property { get; set; }

        public string Value { get; set; }

        [ForeignKey("RuleGroup")]
        public int RuleGroupId { get; set; }

        /// <summary>
        /// For AddTag RuleAction type.
        /// </summary>
        [ForeignKey("Tag")]
        public int? TagId { get; set; }


        public virtual RuleGroup RuleGroup { get; set; }

        public virtual Tag Tag { get; set; }


        public override string ToString()
        {
            switch (RuleActionType)
            {
                case RuleActionTypeEnum.Omit:
                    return $"Omit";
                case RuleActionTypeEnum.AddTag:
                    return (Tag == null) ? $"AddTag({TagId})" : $"AddTag({Tag.Id}, '{Tag.Title}')";
                case RuleActionTypeEnum.SetValueOfProperty:
                    return $"{Property} = '{Value}'";
                case RuleActionTypeEnum.AggregateToMonthlyTransaction:
                    return $"GroupToMonth";
                default:
                    return base.ToString();
            }
        }
    }
}