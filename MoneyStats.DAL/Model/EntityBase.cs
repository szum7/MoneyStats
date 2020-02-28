using System;

namespace MoneyStats.DAL.Model
{
    public class EntityBase
    {
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int State { get; set; }
    }
}
