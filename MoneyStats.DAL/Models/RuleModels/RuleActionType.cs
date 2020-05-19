using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    public enum RuleActionTypeEnum
    {
        Omit = 1,
        AddTag = 2,
        SetValueOfProperty = 3,
        AggregateToMonthlyTransaction = 4
    }

    [Table("RuleActionType")]
    public class RuleActionType : EntityBase
    {
        public string Title { get; set; }
    }
}