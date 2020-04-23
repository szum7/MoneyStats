using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    public enum RuleActionTypeEnum
    {
        Omit,
        AddTags,
        SetValueOfProperty,
        AggregateToATransaction
    }

    [Table("RuleActionType")]
    public class RuleActionType : EntityBase
    {
        public string Title { get; set; }
    }
}