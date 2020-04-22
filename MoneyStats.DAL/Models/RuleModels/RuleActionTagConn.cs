using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    [Table("RuleActionTagConn")]
    public class RuleActionTagConn : EntityBase
    {
        public int RuleActionId { get; set; }
        public int TagId { get; set; }

        public virtual Tag Tag { get; set; }
        public virtual RuleAction RuleAction { get; set; }
    }
}
