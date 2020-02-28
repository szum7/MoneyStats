using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoneyStats.DAL.Model
{
    [Table("TransactionTagConn")]
    public class TransactionTagConn
    {

    }

    [Table("Transaction")]
    public class Transaction : EntityBase
    {
        public int Id { get; set; }
        public DateTime AccountingDate { get; set; }
        public string TransactionId { get; set; } // TODO átnevezni, ne legyen class+id neve
        public string Type { get; set; }
        public string Account { get; set; }
        public string AccountName { get; set; }
        public string PartnerAccount { get; set; }
        public string PartnerName { get; set; }
        public decimal? Sum { get; set; }
        public string Currency { get; set; }
        public string Message { get; set; }

        public virtual ICollection<TransactionTagConn> TransactionTagConn { get; set; }
    }
}
