using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    /// <summary>
    /// More than one rule can apply to a transaction.
    /// </summary>
    [Table("TransactionCreatedWithRule")]
    public class TransactionCreatedWithRule : EntityBase
    {
        public int RuleId { get; set; }
        public int TransactionId { get; set; }

        public virtual Rule Rule { get; set; }
        public virtual Transaction Transaction { get; set; }
    }
}
