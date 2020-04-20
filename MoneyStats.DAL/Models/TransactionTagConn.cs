using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    [Table("TransactionTagConn")]
    public class TransactionTagConn : EntityBase
    {
        [ForeignKey("Transaction")]
        public int TransactionId { get; set; }

        [ForeignKey("Tag")]
        public int TagId { get; set; }


        public virtual Transaction Transaction { get; set; }

        public virtual Tag Tag { get; set; }
    }
}
