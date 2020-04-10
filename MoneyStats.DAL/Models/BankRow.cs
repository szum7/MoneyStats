using MoneyStats.DAL.Common;
using System;
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
        public BankType BankType { get; set; }

        /// <summary>
        /// For aggregated transactions. E.g.: monthly food.
        /// One transaction can only be in one group! Being in 
        /// multiple groups would mean counting it multiple 
        /// times, messing up charts/results/etc.
        /// </summary>
        [ForeignKey("Transaction")]
        public int? TransactionGroupId { get; set; }


        #region K&H Bank columns
        [Rulable]
        public DateTime? AccountingDate { get; set; }

        [Rulable]
        public string TransactionId { get; set; }

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
}
