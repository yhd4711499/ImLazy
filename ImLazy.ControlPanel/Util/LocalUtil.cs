using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImLazy.ControlPanel.Util
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
            if (key == null)
                return null;
            var s = Properties.Resources.ResourceManager.GetString(key);
            return s ?? String.Format("[{0}]", key);
        }

        /// <summary>
        /// Get localized string
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns>[xxx] if no matched resource is found</returns>
        public static string LocalError(this int errorCode)
        {
            var key = String.Format("ERR_0x{0:x8}", errorCode);
            var s = Properties.Resources.ResourceManager.GetString(key);
            return s ?? String.Format("[{0}]", key);
        }
    }
}
