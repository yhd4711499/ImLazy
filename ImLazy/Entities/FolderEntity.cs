using System.Collections.Generic;

namespace ImLazy.Entities
{
    public class FolderEntity
    {
        public FolderEntity()
        {
            RuleProperties = new HashSet<RulePropertyEntity>();
        }

        public int Id { get; set; }
        public string FolderPath { get; set; }
        public bool Enabled { get; set; }

        public virtual ICollection<RulePropertyEntity> RuleProperties { get; set; }
    }
}