using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    /// <summary>
    /// Transaction - BankRow is an n-n connection
    /// because imagine the following example:
    /// You want to aggregate up transaction regarding food monthly AND yearly.
    /// There will be transactions part of the monthly aggregated transactions and the yearly aggregated one.
    /// Plus an aggregated transaction has more then one transactions - it's aggregated.
    /// 
    /// In other words:
    /// - Transaction has many BankRows -> Transaction is a group
    /// - BankRows has many Tranasctions -> Bankrow is in multiple groups
    /// </summary>
    [Table("TransactionBankRowConn")]
    public class TransactionBankRowConn : EntityBase
    {
        public int TransactionId { get; set; }
        public int BankRowId { get; set; }

        public virtual Transaction Transaction { get; set; }
        public virtual BankRow BankRow { get; set; }
    }
}
