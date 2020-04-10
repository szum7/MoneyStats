using MoneyStats.DAL.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    [Table("Transaction")]
    public partial class Transaction : EntityBase // (K&H exported transactions)
    {
        [Rulable]
        public DateTime AccountingDate { get; set; }

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

        /// <summary>
        /// This is a foreign key in the same table where the reference is.
        /// 
        /// Ezzel lehet összesíteni tranzakciókat, pl élelmiszerre költött kiadásokat.
        /// 
        /// A hivatkozott tranzakció egy összegzett Sum értékkel és hóvégi dátummal.
        /// A további propertyk ajánlatot tesz majd a program, de valószínűleg
        /// gyakran át fogják írni.
        /// 
        /// Összesítéseknél/kimutatásoknál vagy a hivatkozott, vagy azokat a tranzakciókat 
        /// használjuk, melyeknél ez a property nem null.
        /// 
        /// Rule kiértékelésnél a hivatkozott sor alatt egymás után listázzuk a hivatkozókat.
        /// (null értékű alatt a nem null értékűek.)
        /// </summary>
        public int TransactionGroupParentId { get; set; }

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
