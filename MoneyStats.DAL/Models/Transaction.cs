using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    [Table("Transaction")]
    public class Transaction : EntityBase
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int Sum { get; set; }

        /// <summary>
        /// This means there are more than one bank transaction associated with this.
        /// E.g.: a monthly sum of transactions relating food.
        /// This is for performance purposes, could just check if the Id exists 
        /// in the connection table (TransactionBankRowConn).
        /// </summary>
        public bool IsGroup { get; set; }

        public int? BankTransactionId { get; set; }


        public virtual BankRow BankTransaction { get; set; }
    }
}
