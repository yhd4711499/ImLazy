using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImLazy.Entities
{
    public class RuleEntity
    {
        public RuleEntity()
        {
            Actions = new HashSet<ActionAddinInfoEntity>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int ConditionBranchId { get; set; }
        public virtual ICollection<ActionAddinInfoEntity> Actions { get; set; }
        [ForeignKey("ConditionBranchId")]

        public virtual ConditionBranchEntity ConditionBranch { get; set; }
    }
}