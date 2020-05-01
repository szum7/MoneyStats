using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    [Table("Tag")]
    public class Tag : EntityBase
    {
        public string Title { get; set; }

        public string Description { get; set; }


        public virtual List<TransactionTagConn> TransactionTagConns { get; set; }
        public virtual List<RuleActionTagConn> RuleActionTagConns { get; set; }

        public Tag()
        {
            TransactionTagConns = new List<TransactionTagConn>();
            RuleActionTagConns = new List<RuleActionTagConn>();
        }
    }
}
