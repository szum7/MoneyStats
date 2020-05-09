using MoneyStats.DAL.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    /// <summary>
    /// This is a table for unmodified bank transactions.
    /// The content of this table is strictly for reference purposes. We do not even
    /// make calculations based on the Sum property. These important values are copied
    /// over to the inner transaction type (currently named "Transaction", 2020-04-10)
    /// 
    /// K&H bank has it's own set of columns in it's exported excels -> e.g.: A, B, C
    /// CIB has another set, matching columns can occur too -> e.g.: A, D, E, F, C
    /// ...
    /// This table has the complete set of columns -> A, B, C, D, E, F
    /// Example: if it's a K&H bank transaction, D, E, F columns will definitely be null.
    /// </summary>
    [Table("BankRow")]
    public partial class BankRow : EntityBase
    {
        /// <summary>
        /// Not yet in use.
        /// </summary>
        [NotMapped]
        public BankType BankType { get; set; }

        /// <summary>
        /// The workflow is:
        /// 1. read bank exported excel transactions
        /// 2. save bank rows
        /// 3. list newly inserted bank rows and create transactions based on them (use rules if desired)
        /// Connection could be lost, page refreshed or other disruptive action done
        /// to discontinue the workflow. This bool tells us if the current bank row 
        /// has undergone the 3th step.
        /// </summary>
        public bool IsTransactionCreated { get; set; }

        /// <summary>
        /// For aggregated transactions. E.g.: monthly food.
        /// One transaction can only be in one group! Being in 
        /// multiple groups would mean counting it multiple 
        /// times, messing up charts/results/etc.
        /// Many bank rows references one transaction.
        /// Aggregated bank rows generate only one transaction.
        /// 
        /// Note:
        /// Not a foreign key because EF Core Code First is shit
        /// and can't make sence of circular references.
        /// </summary>
        public int? TransactionGroupId { get; set; }
        public virtual Transaction TransactionGroup { get; set; }


        #region K&H Bank columns
        [Rulable]
        public DateTime? AccountingDate { get; set; }

        [Rulable]
        public string BankTransactionId { get; set; }

        [Rulable]
        public string Type { get; set; }

        [Rulable]
        public string Account { get; set; }

        [Rulable]
        public string AccountName { get; set; }

        [Rulable]
        public string PartnerAccount { get; set; }

        [Rulable]
        public string PartnerName { get; set; }

        [Rulable]
        public decimal? Sum { get; set; }

        [Rulable]
        public string Currency { get; set; }

        [Rulable]
        public string Message { get; set; }
        #endregion


        #region OTP Bank columns
        #endregion
    }

    public partial class BankRow
    {
        [NotMapped]
        public string ContentId => $"{AccountingDate}{BankTransactionId}{Type}{Account}{AccountName}{PartnerAccount}{PartnerName}{Sum?.ToString()}{Currency}{Message}";
    }
}
