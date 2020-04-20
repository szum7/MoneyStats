using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoneyStats.DAL.Models.Extensions
{
    public partial class Transaction
    {
        [NotMapped]
        public List<Tag> Tags { get; set; }
    }
}
