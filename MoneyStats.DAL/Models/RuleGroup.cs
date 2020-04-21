using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    [Table("RuleGroup")]
    public class RuleGroup : EntityBase
    {
        public List<Rule> Rules { get; set; }
        public List<RuleAction> RuleActions { get; set; }
    }
}