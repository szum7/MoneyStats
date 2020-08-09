using MoneyStats.DAL.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    [Table("Transaction")]
    public partial class Transaction : EntityBase
    {
        [Rulable]
        public string Title { get; set; }

        /// <summary>
        /// Title explained. Useful for IsCustom=true transactions.
        /// </summary>
        [Rulable]
        public string Description { get; set; }

        public DateTime? Date { get; set; }

        public decimal? Sum { get; set; }

        public string AppliedRules { get; set; }

        /// <summary>
        /// We can create transactions without bank exported transaction reference.
        /// E.g.: Create an IsCutom=true transaction for transactions payed with cash,
        /// where there's no record.
        /// NOTE! defaultValue: false
        /// </summary>
        public bool IsCustom { get; set; }

        /// <summary>
        /// Reference from which the Transaction was created from.
        /// </summary>
        [ForeignKey("BankRow")]
        public int? BankRowId { get; set; }


        public virtual BankRow BankRow { get; set; }
        public virtual List<TransactionTagConn> TransactionTagConn { get; set; }
    }

    public partial class Transaction
    {
        [NotMapped]
        public List<Tag> Tags { get; set; }

        /// <summary>
        /// For aggregated BankRows.
        /// If it has elements, BankRow should be null.
        /// </summary>
        [NotMapped]
        public List<BankRow> AggregatedReferences { get; set; }

        /// <summary>
        /// This means there are more than one bank transaction associated with this.
        /// E.g.: a monthly sum of transactions relating food.
        /// If true, BankTransactionId MUST BE null! (Otherwise -> circular reference)
        /// NOTE! defaultValue: false
        /// </summary>
        [NotMapped]
        public bool IsGroup => !BankRowId.HasValue;

        [NotMapped]
        public string FancyName => $"[{this.BaseFancyName}][{Title}][{Description}][{Date?.ToString("yyyy-MM-dd")}][{Sum}][{IsCustom}][{BankRowId}]";

        public Transaction()
        {
            this.Tags = new List<Tag>();
            this.AggregatedReferences = new List<BankRow>();
            this.TransactionTagConn = new List<TransactionTagConn>();
        }
    }
}
