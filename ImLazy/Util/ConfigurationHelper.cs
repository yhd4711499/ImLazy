﻿using System.Collections.Generic;
using log4net;

namespace ImLazy.Util
{
    public static class ConfigurationHelper
    {

        static readonly ILog Log = LogManager.GetLogger(typeof(ConfigurationHelper));

        public static T TryGetValue<T>(this Dictionary<string, object> dic, string key)
        {
            object v;
            if (dic.TryGetValue(key, out v)) return (T) v;
            Log.WarnFormat("Config ({0}) not found! Return null or default instead.", key);
            return default(T);
        }

        public static object TryGetValue(this Dictionary<string, object> dic, string key)
        {
            object v;
            return !dic.TryGetValue(key, out v) ? null : v;
        }
    }
}