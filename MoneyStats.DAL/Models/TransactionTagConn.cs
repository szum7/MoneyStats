using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    [Table("TransactionTagConn")]
    public class TransactionTagConn
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int TagId { get; set; }

        public virtual Tag Tag { get; set; }
        public virtual Transaction Transaction { get; set; }
    }
}
