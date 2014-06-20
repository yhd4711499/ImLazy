using System;

namespace ImLazy.Entities
{
    public class RulePropertyEntity
    {
        public int Id { get; set; }
        public bool Enabled { get; set; }
        public int Priority { get; set; }
        public int FolderEntityId { get; set; }
        public Guid RuleId { get; set; }
    }
}