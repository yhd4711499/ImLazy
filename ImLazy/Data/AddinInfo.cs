using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ImLazy.Entities;
using ImLazy.Util;

namespace ImLazy.Data
{
    [Serializable]
    public abstract class AddinInfo : DataItemBase<AddinInfoEntity>, IAddinInfo
    {
        public string AddinType { get; set; }
        [NotMapped]
        public string LocalName { get; set; }

        public virtual void Save(SerializableDictionary<string, object> config)
        {
            Config = config;
        }
        [NotMapped]
        public SerializableDictionary<string, object> Config { get; set; }

        internal AddinInfo()
        {
            Config = new SerializableDictionary<string, object>();
        }

        protected abstract AddinInfoEntity GetDerivedEntity();

        public override AddinInfoEntity GetEntity()
        {
            var derived = GetDerivedEntity();
            derived.AddinType = AddinType;
            derived.Configs = Config.ToEntities();
            return derived;
        }

        public override void FromEntity(AddinInfoEntity entity, ModelContainer context)
        {
            AddinType = entity.AddinType;
            var configs = entity.Configs;
            var dic = new SerializableDictionary<string, object>();
            foreach (var configEntity in configs)
            {
                dic.Add(configEntity.Key, configEntity.Value);
            }
            Config = dic;
        }
    }
}