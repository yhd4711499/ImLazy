using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ImLazy.Data;
using ImLazy.Util;

namespace ImLazy.Entities
{
    public class AddinInfoEntity : IAddinInfo
    {
        public AddinInfoEntity()
        {
            Configs = new HashSet<ConfigEntity>();
        }

        public int Id { get; set; }
        public virtual ICollection<ConfigEntity> Configs { get; set; }
        public string AddinType { get; set; }

        [NotMapped]
        public string LocalName { get; set; }

        [NotMapped]
        public SerializableDictionary<string, object> Config { get; set; }

        public void Save(SerializableDictionary<string, object> config)
        {
            Config = config;
            Configs = config.ToEntities();
        }


        public object Clone()
        {
            return ObjectCopier.Clone(this);
        }
    }
}