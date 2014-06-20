using System.Collections.Generic;
using ImLazy.Entities;

namespace ImLazy.Util
{
    public static class ConfigurationHelper
    {
        //static readonly ILog Log = LogManager.GetLogger(typeof(ConfigurationHelper));

        public static T TryGetValue<T>(this Dictionary<string, object> dic, string key)
        {
            if (dic == null)
                return default(T);
            object v;
            if (dic.TryGetValue(key, out v)) return (T) v;
            //Log.WarnFormat("Config ({0}) not found! Return null or default instead.", key);
            return default(T);
        }

        public static string TryGetValue<T>(this Dictionary<string, string> dic, string key)
        {
            if (dic == null)
                return null;
            string v;
            if (dic.TryGetValue(key, out v)) return v;
            //Log.WarnFormat("Config ({0}) not found! Return null or default instead.", key);
            return null;
        }

        public static string TryGetValue(this Dictionary<string, string> dic, string key)
        {
            string v;
            return !dic.TryGetValue(key, out v) ? null : v;
        }

        public static object TryGetValue(this Dictionary<string, object> dic, string key)
        {
            object v;
            return !dic.TryGetValue(key, out v) ? null : v;
        }

        public static ICollection<ConfigEntity> ToEntities(this Dictionary<string, object> dic)
        {
            var entities = new List<ConfigEntity>(dic.Count);
            dic.ForEach(pair => entities.Add(new ConfigEntity{Key = pair.Key, Value = pair.Value == null? null : pair.Value.ToString()}));
            return entities;
        }
    }
}
