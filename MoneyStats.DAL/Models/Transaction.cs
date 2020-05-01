using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    [Table("Transaction")]
    public partial class Transaction : EntityBase
    {
        public string Title { get; set; }

        /// <summary>
        /// Title explained. Useful for IsCustom=true transactions.
        /// </summary>
        public string Description { get; set; }

        public DateTime? Date { get; set; }

        public decimal? Sum { get; set; }

        /// <summary>
        /// This means there are more than one bank transaction associated with this.
        /// E.g.: a monthly sum of transactions relating food.
        /// This is for performance purposes, could just check if the Id exists 
        /// in the connection table (TransactionBankRowConn).
        /// If true, BankTransactionId MUST BE null! (Otherwise -> circular reference)
        /// NOTE! defaultValue: false
        /// </summary>
        public bool IsGroup { get; set; }

        /// <summary>
        /// We can create transactions without bank exported transaction reference.
        /// E.g.: Create an IsCutom=true transaction for transactions payed with cash,
        /// where there's no record.
        /// NOTE! defaultValue: false
        /// </summary>
        public bool IsCustom { get; set; }

        /// <summary>
        /// Reference.
        /// </summary>
        [ForeignKey("BankRow")]
        public int? BankRowId { get; set; }


        public virtual BankRow BankTransaction { get; set; }
        public virtual List<BankRow> BankRows { get; set; }
        public virtual List<TransactionTagConn> TransactionTagConn { get; set; }
        public virtual List<TransactionCreatedWithRule> TransactionCreatedWithRule { get; set; }
    }

    public partial class Transaction
    {
        [NotMapped]
        public List<Tag> Tags { get; set; }

        public Transaction()
        {
            this.Tags = new List<Tag>();
            this.BankRows = new List<BankRow>();
            this.TransactionCreatedWithRule = new List<TransactionCreatedWithRule>();
            this.TransactionTagConn = new List<TransactionTagConn>();
        }
    }
}
