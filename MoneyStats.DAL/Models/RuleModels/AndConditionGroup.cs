using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    /// <summary>
    /// Example 1: AndConditionGroup = a
    /// Example 2: AndConditionGroup = a && b && c
    /// Example 3: AndConditionGroup = a && b
    /// </summary>
    [Table("AndConditionGroup")]
    public partial class AndConditionGroup : EntityBase
    {
        public int RuleId { get; set; }

        public virtual Rule Rule { get; set; }
        public virtual List<Condition> Conditions { get; set; }
    }

    public partial class AndConditionGroup
    {
        public AndConditionGroup()
        {
            this.Conditions = new List<Condition>();
        }

        public override string ToString()
        {
            if (Conditions.Count == 0)
                return base.ToString();

            return string.Join(" && ", Conditions);
        }
    }
}