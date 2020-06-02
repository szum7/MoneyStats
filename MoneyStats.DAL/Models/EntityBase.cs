using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    public abstract partial class EntityBase
    {
        [Key]
        public int Id { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public int State { get; set; }
    }

    public abstract partial class EntityBase
    {
        [NotMapped]
        public bool IsActive => this.State == 1;

        [NotMapped]
        public bool IsDeleted => this.State == 0;

        [NotMapped]
        public string BaseFancyName => $"{Id},{State}";
    }

    public static class EntityBaseExtension
    {
        public static Type SetNew<Type>(this Type target) where Type : EntityBase
        {
            target.CreateDate = DateTime.Now;
            target.State = 1;
            return target;
        }
    }
}
