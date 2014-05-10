using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfLocalization;

namespace ImLazy.Addins.Utils
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
            var s = Properties.Resources.ResourceManager.GetString(key.Replace('.','_'));
            return s ?? String.Format("[{0}]", key);
        }

        public static LocalString ToLocalString(this object obj)
        {
            return (LocalString) obj;
        }

        public static LocalString LocalString(this string key)
        {
            return new LocalString(key, key.Local());
        }
    }
}
