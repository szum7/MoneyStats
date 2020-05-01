using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    [Table("Tag")]
    public class Tag : EntityBase
    {
        public string Title { get; set; }

        public string Description { get; set; }


        public virtual ICollection<TransactionTagConn> TransactionTagConns { get; set; }
        public virtual ICollection<RuleActionTagConn> RuleActionTagConns { get; set; }

        public Tag()
        {
            TransactionTagConns = new HashSet<TransactionTagConn>();
            RuleActionTagConns = new HashSet<RuleActionTagConn>();
        }
    }
}
