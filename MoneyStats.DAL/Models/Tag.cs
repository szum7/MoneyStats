using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    [Table("Tag")]
    public class Tag : EntityBase
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public virtual ICollection<TransactionTagConn> TransactionTagConn { get; set; }

        public Tag()
        {
            TransactionTagConn = new HashSet<TransactionTagConn>();
        }
    }
}
