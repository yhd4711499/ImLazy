using System;
using log4net;

namespace ImLazy.Util
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
    }
}
