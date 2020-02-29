using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    public partial class EntityBase
    {
        [NotMapped]
        public bool IsActive => this.State == 1;

        [NotMapped]
        public bool IsDeleted => this.State == 0;
    }
}
