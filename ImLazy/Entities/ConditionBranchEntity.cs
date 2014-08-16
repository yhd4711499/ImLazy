using System.Collections.Generic;

namespace ImLazy.Entities
{
    public class ConditionBranchEntity : ConditionCorpEntity
    {
        public ConditionBranchEntity()
        {
            SubConditions = new HashSet<ConditionCorpEntity>();
        }

        public string Mode { get; set; }
        public virtual ICollection<ConditionCorpEntity> SubConditions { get; set; }
    }
}