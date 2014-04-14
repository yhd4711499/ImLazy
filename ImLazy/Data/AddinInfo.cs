using System;
using System.Collections.Generic;

namespace ImLazy.Data
{
    [Serializable]
    public class AddinInfo : DataItemBase, IAddinInfo
    {
        public string AddinType { get; set; }
        public string Name { get; set; }

        public virtual void Save(SerializableDictionary<string, object> config)
        {
            Config = config;
        }
        public SerializableDictionary<string, object> Config { get; set; }

        internal AddinInfo()
        {
            Config = new SerializableDictionary<string, object>();
        }
    }
}