using System;
using System.Collections.Generic;

namespace ImLazy.Data
{
    public interface IAddinInfo : ICloneable
    {
        string AddinType { get; set; }
        string Name { get; set; }
        SerializableDictionary<string, object> Config { get; set; }
        void Save(SerializableDictionary<string, object> config);
    }
}