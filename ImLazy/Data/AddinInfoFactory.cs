using System.Collections.Generic;
using CuttingEdge.Conditions;
using ImLazy.SDK.Base.Contracts;

namespace ImLazy.Data
{
    public static class AddinInfoFactory
    {
/*        public static T Create<T>(IAddinMetadata metadata) where T : IAddinInfo, new()
        {
            return new T
            {
                AddinType = metadata.Type.Name,
                LocalName = metadata.LocalName,
                Config = new SerializableDictionary<string, object>()
            };
        }*/

        internal static T Create<T>(IAddin addin) where T : IAddinInfo, new()
        {
            Condition.Requires(addin, "addin").IsNotNull();
            
            return new T
            {
                AddinType = addin.GetType().FullName,//metadata.Type.Name,
                LocalName = addin.LocalName,
                Config = new SerializableDictionary<string, object>()
            };
        }
    }
}