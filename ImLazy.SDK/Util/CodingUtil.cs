using System;
using System.Linq;
using log4net;

namespace ImLazy.SDK.Util
{
    public static class CodingUtil
    {
        public static void CheckParamOrThrow(object param, string nullMsg)
        {
            if (param == null)
                throw new ArgumentNullException(nullMsg);
        }

        public static bool CheckParam(this ILog log, object param, string nullMsgFormat, params object[] args)
        {
            if (param != null) return true;
            log.ErrorFormat(nullMsgFormat, args);
            return false;
        }
        public static bool CheckParams(this ILog log, string nullMsgFormat, params object[] paramsObjects)
        {
            foreach (var p in paramsObjects.Where(p => p == null))
            {
                log.ErrorFormat(nullMsgFormat, p);
                return false;
            }
            return true;
        }
    }
}
