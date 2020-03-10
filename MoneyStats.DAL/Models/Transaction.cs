using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    [Table("Transaction")]
    public partial class Transaction : EntityBase // (K&H exported transactions)
    {
        public DateTime AccountingDate { get; set; }
        public string TransactionId { get; set; }
        public string Type { get; set; }
        public string Account { get; set; }
        public string AccountName { get; set; }
        public string PartnerAccount { get; set; }
        public string PartnerName { get; set; }
        public decimal? Sum { get; set; }
        public string Currency { get; set; }
        public string Message { get; set; }
        public string OriginalContentId { get; set; }
        /// <summary>
        /// User can set an id. Useful when user wants to add a transaction by hand, 
        /// not read from an excel file.
        /// </summary>
        public string CustomId { get; set; }

        public virtual ICollection<TransactionTagConn> TransactionTagConn { get; set; }

        public Transaction()
        {
            TransactionTagConn = new HashSet<TransactionTagConn>();
        }
    }
}
