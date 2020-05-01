using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MoneyStats.DAL.Models
{
    /// <summary>
    /// RuleActions are not stored to be used multiple types. They belong
    /// to exactly one RuleGroup.
    /// </summary>
    [Table("RuleAction")]
    public partial class RuleAction : EntityBase
    {
        public string Title { get; set; }
        [ForeignKey("RuleActionType")]
        public int RuleActionTypeId { get; set; }
        public string Property { get; set; }
        public string Value { get; set; }
        [ForeignKey("RuleGroup")]
        public int RuleGroupId { get; set; }

        public virtual RuleGroup RuleGroup { get; set; }
        public virtual RuleActionType RuleActionType { get; set; }
        public virtual List<RuleActionTagConn> RuleActionTagConns { get; set; }
    }

    public partial class RuleAction
    {
        /// <summary>
        /// For "AddTags" type of action
        /// </summary>
        [NotMapped]
        public List<Tag> TagsToBeApplied
        {
            get
            {
                if (_tagsToBeApplied.Count != RuleActionTagConns.Count)
                {
                    foreach (var conn in RuleActionTagConns)
                    {
                        if (conn.Tag == null)
                            continue;

                        _tagsToBeApplied.Add(conn.Tag);
                    }
                }
                return _tagsToBeApplied;
            }
        }
        private List<Tag> _tagsToBeApplied;

        public RuleAction()
        {
            this._tagsToBeApplied = new List<Tag>();
            this.RuleActionTagConns = new List<RuleActionTagConn>();
        }
    }
}