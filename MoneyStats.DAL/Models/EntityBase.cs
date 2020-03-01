using System;

namespace MoneyStats.DAL.Models
{
    public abstract partial class EntityBase
    {
        public int Id { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int State { get; set; }
    }
}
