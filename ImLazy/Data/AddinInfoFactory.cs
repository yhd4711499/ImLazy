using System.Collections.Generic;
using ImLazy.Contracts;

namespace ImLazy.Data
{
    public static class AddinInfoFactory
    {
        public static T Create<T>(IAddinMetadata metadata) where T : IAddinInfo, new()
        {
            return new T
            {
                AddinType = metadata.Type.FullName,
                Name = metadata.Name,
                Config = new SerializableDictionary<string, object>()
            };
        }
    }
}