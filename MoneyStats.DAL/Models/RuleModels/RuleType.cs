using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    public enum RuleTypeEnum
    {
        TrueRule,
        HasValueOfProperty,
        IsPropertyNull,
        IsPropertyNotNull,
        ContainsValueOfProperty // property's value contains a string
    }

    [Table("RuleType")]
    public class RuleType : EntityBase
    {
        public string Title { get; set; }
    }
}
