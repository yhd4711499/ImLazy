using System;

namespace ImLazy.Util
{
    internal static class LocalUtil
    {
        /// <summary>
        /// Get localized string
        /// </summary>
        /// <param name="key"></param>
        /// <returns>[xxx] if no matched resource is found</returns>
        public static string Local(this string key)
        {
            var s = Properties.Resources.ResourceManager.GetString(key);
            return s ?? String.Format("[{0}]", key);
        }
    }
}
